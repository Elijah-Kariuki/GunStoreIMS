using AutoMapper;
using GunStoreIMS.Domain.Interfaces;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GunStoreIMS.Application.Services
{
    public class DealerRecordService : IDealerRecordService
    {
        private readonly FirearmsInventoryDB _db;
        private readonly IMapper _mapper;

        public DealerRecordService(FirearmsInventoryDB db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private void ValidateDealerLicenseExpiration(DateTime expirationDate)
        {
            if (expirationDate <= DateTime.Today)
                throw new ValidationException("Dealer license expiration must be in the future.");
        }

        public async Task AddDealerRecordAsync(DealerRecord dealerRecord)
        {
            ValidateDealerLicenseExpiration(dealerRecord.LicenseExpirationDate);
            dealerRecord.Id = Guid.NewGuid();
            _db.DealerRecords.Add(dealerRecord);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> UpdateDealerRecordAsync(DealerRecord dealerRecord)
        {
            ValidateDealerLicenseExpiration(dealerRecord.LicenseExpirationDate);
            var existing = await _db.DealerRecords.FindAsync(dealerRecord.Id);
            if (existing == null) return false;

            _db.Entry(existing).CurrentValues.SetValues(dealerRecord);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDealerRecordAsync(Guid id)
        {
            var existing = await _db.DealerRecords.FindAsync(id);
            if (existing == null) return false;

            _db.DealerRecords.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<DealerRecord?> GetDealerRecordByIdAsync(Guid id)
            => await _db.DealerRecords.FindAsync(id);

        public async Task<IEnumerable<DealerRecord>> GetAllDealerRecordsAsync()
            => await _db.DealerRecords.ToListAsync();

        public async Task<IEnumerable<DealerRecord>> GetRecordsByTradeNameAsync(string tradeName)
            => await _db.DealerRecords
                        .Where(dr => EF.Functions.Like(dr.TradeName, $"%{tradeName}%"))
                        .ToListAsync();

        public async Task<IEnumerable<DealerRecord>> GetRecordsByFflNumberAsync(string fflNumber)
            => await _db.DealerRecords
                        .Where(dr => dr.FFLNumber == fflNumber)
                        .ToListAsync();
    }
}
