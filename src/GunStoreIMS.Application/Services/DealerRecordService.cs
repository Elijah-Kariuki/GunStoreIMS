using AutoMapper;
using GunStoreIMS.Abstractions.Interfaces;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GunStoreIMS.Application.Services
{
    public class DealerRecordService : IDealerRecordService
    {
        private readonly FirearmsInventoryDB _db;
        // ℹ️ No IMapper needed if service only handles domain models

        public DealerRecordService(FirearmsInventoryDB db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // Internal validation helper
        private (bool IsValid, string? Error) ValidateDealer(DealerRecord dealerRecord)
        {
            if (dealerRecord.IsActive && dealerRecord.ExpirationDateUtc.Date < DateTime.UtcNow.Date)
            {
                return (false, "An active FFL license cannot be expired.");
            }
            // Add other business validations here...
            return (true, null);
        }

        public async Task<DealerRecord?> GetDealerRecordByIdAsync(Guid id) =>
            await _db.DealerRecords.FindAsync(id);

        public async Task<IEnumerable<DealerRecord>> GetAllDealerRecordsAsync(bool? isActive = null)
        {
            var query = _db.DealerRecords.AsQueryable();
            if (isActive.HasValue)
            {
                query = query.Where(r => r.IsActive == isActive.Value);
            }
            return await query.OrderBy(r => r.TradeName).ToListAsync();
        }

        public async Task<IEnumerable<DealerRecord>> GetRecordsByTradeNameAsync(string tradeName) =>
            await _db.DealerRecords
                     .Where(dr => EF.Functions.Like(dr.TradeName, $"%{tradeName}%"))
                     .ToListAsync();

        public async Task<IEnumerable<DealerRecord>> GetRecordsByFflNumberAsync(string fflNumber) =>
            await _db.DealerRecords
                     .Where(dr => dr.FFLNumber == fflNumber)
                     .ToListAsync();

        public async Task<(DealerRecord?, string?)> AddDealerRecordAsync(DealerRecord dealerRecord)
        {
            if (await _db.DealerRecords.AnyAsync(dr => dr.FFLNumber == dealerRecord.FFLNumber))
            {
                return (null, $"Conflict: Dealer with FFL number {dealerRecord.FFLNumber} already exists.");
            }

            dealerRecord.IsActive = true; // Ensure active on creation
            var (isValid, error) = ValidateDealer(dealerRecord);
            if (!isValid) return (null, $"Validation Error: {error}");

            dealerRecord.Id = Guid.NewGuid();
            dealerRecord.LastUpdatedUtc = DateTime.UtcNow;
            dealerRecord.RecordDate = DateTime.UtcNow.Date;

            _db.DealerRecords.Add(dealerRecord);
            await _db.SaveChangesAsync();
            return (dealerRecord, null);
        }

        public async Task<(bool, string?)> UpdateDealerRecordAsync(DealerRecord dealerRecord)
        {
            var existing = await _db.DealerRecords.AsNoTracking().FirstOrDefaultAsync(d => d.Id == dealerRecord.Id);
            if (existing == null) return (false, "Not Found: Dealer record not found.");

            var (isValid, error) = ValidateDealer(dealerRecord);
            if (!isValid) return (false, $"Validation Error: {error}");

            dealerRecord.LastUpdatedUtc = DateTime.UtcNow; // Ensure timestamp updates

            _db.DealerRecords.Update(dealerRecord); // Use Update for tracked entities

            try
            {
                await _db.SaveChangesAsync();
                return (true, null);
            }
            catch (DbUpdateException ex)
            {
                return (false, $"Database Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(bool, string?)> ArchiveDealerRecordAsync(Guid id)
        {
            var record = await _db.DealerRecords.FindAsync(id);
            if (record == null) return (false, "Not Found: Dealer record not found.");
            if (!record.IsActive) return (true, "Record already archived.");

            record.IsActive = false;
            record.LastUpdatedUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool, string?)> ActivateDealerRecordAsync(Guid id)
        {
            var record = await _db.DealerRecords.FindAsync(id);
            if (record == null) return (false, "Not Found: Dealer record not found.");
            if (record.IsActive) return (true, "Record already active.");

            if (record.ExpirationDateUtc.Date < DateTime.UtcNow.Date)
            {
                return (false, "Bad Request: Cannot activate an FFL with an expired license.");
            }

            record.IsActive = true;
            record.LastUpdatedUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<bool> DeleteDealerRecordAsync(Guid id)
        {
            // Prefer Archive, but implement Delete if absolutely needed.
            var existing = await _db.DealerRecords.FindAsync(id);
            if (existing == null) return false;
            // 🚨 Add checks here to prevent deleting FFLs linked to transactions!
            _db.DealerRecords.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}