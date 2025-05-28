// File: GunStoreIMS.Persistence/Repositories/EfFirearmRepository.cs
using GunStoreIMS.Abstractions.Interfaces;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using GunStoreIMS.Shared.Enums; // For FirearmStatus
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GunStoreIMS.Persistence.Repositories
{
    public class EfFirearmRepository : IFirearmRepository
    {
        private readonly FirearmsInventoryDB _context;

        public EfFirearmRepository(FirearmsInventoryDB context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Firearm?> GetByIdAsync(Guid id)
        {
            return await _context.Firearms
                                 .Include(f => f.Caliber) // Example of eager loading
                                 .Include(f => f.DealerRecord) // Example
                                 .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<(IEnumerable<Firearm> Items, int TotalCount)> QueryAsync(
            FirearmStatus status, int page, int pageSize)
        {
            var query = _context.Firearms
                                .Include(f => f.Caliber) // Eager load for list display
                                .Where(f => f.CurrentStatus == status);

            var totalCount = await query.CountAsync();

            var items = await query
                                .OrderByDescending(f => f.InitialAcquisitionDate) // Or another relevant field
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            return (items, totalCount);
        }

        public async Task AddAsync(Firearm entity)
        {
            await _context.Firearms.AddAsync(entity);
        }

        public void Update(Firearm entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> ExistsBySerialNumberAsync(string serialNumber, string manufacturer, string model, string? importerName = null)
        {
            if (string.IsNullOrEmpty(importerName))
            {
                return await _context.Firearms.AnyAsync(f =>
                    f.SerialNumber == serialNumber &&
                    f.Manufacturer == manufacturer &&
                    f.Model == model &&
                    f.ImporterName == null);
            }
            else
            {
                return await _context.Firearms.AnyAsync(f =>
                    f.SerialNumber == serialNumber &&
                    f.Manufacturer == manufacturer &&
                    f.Model == model &&
                    f.ImporterName == importerName);
            }
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}