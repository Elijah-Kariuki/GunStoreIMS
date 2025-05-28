// File: GunStoreIMS.Persistence/Repositories/EfAcquisitionRecordRepository.cs
using GunStoreIMS.Abstractions.Interfaces;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GunStoreIMS.Persistence.Repositories
{
    public class EfAcquisitionRecordRepository : IAcquisitionRecordRepository
    {
        private readonly FirearmsInventoryDB _context;

        public EfAcquisitionRecordRepository(FirearmsInventoryDB context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<AcquisitionRecord?> GetByIdAsync(Guid id)
        {
            return await _context.AcquisitionRecords
                                 // .Include(ar => ar.Firearm) // Optional: Eager load related Firearm
                                 .FirstOrDefaultAsync(ar => ar.Id == id);
        }

        public async Task<IEnumerable<AcquisitionRecord>> GetAllAsync()
        {
            return await _context.AcquisitionRecords
                                 // .Include(ar => ar.Firearm) // Optional
                                 .OrderByDescending(r => r.AcquisitionDate) // Matches controller logic
                                 .ToListAsync();
        }

        public async Task AddAsync(AcquisitionRecord entity)
        {
            await _context.AcquisitionRecords.AddAsync(entity);
        }

        public void Update(AcquisitionRecord entity)
        {
            // EF Core tracks changes on the entity.
            // If the entity is already tracked, simply calling SaveChangesAsync will persist them.
            // If it's a detached entity, you might need:
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.AcquisitionRecords.FindAsync(id);
            if (entity != null)
            {
                _context.AcquisitionRecords.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.AcquisitionRecords.AnyAsync(ar => ar.Id == id);
        }

        public async Task<AcquisitionRecord?> FindBySerialNumberAsync(string serialNumber)
        {
            // This assumes SerialNumber on AcquisitionRecord is unique or you want the first.
            // Your controller checks this. If SerialNumber belongs to Firearm, adjust query.
            return await _context.AcquisitionRecords
                .FirstOrDefaultAsync(r => r.SerialNumber == serialNumber);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}