// File: GunStoreIMS.Persistence/Repositories/EfDispositionRecordRepository.cs
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
    public class EfDispositionRecordRepository : IDispositionRecordRepository
    {
        private readonly FirearmsInventoryDB _context;

        public EfDispositionRecordRepository(FirearmsInventoryDB context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DispositionRecord?> GetByIdAsync(Guid id)
        {
            return await _context.DispositionRecords
                                 // .Include(dr => dr.Firearm) // Optional
                                 .FirstOrDefaultAsync(dr => dr.Id == id);
        }

        public async Task<IEnumerable<DispositionRecord>> GetAllAsync()
        {
            return await _context.DispositionRecords
                                 // .Include(dr => dr.Firearm) // Optional
                                 .OrderByDescending(r => r.TransactionDate) // Matches controller
                                 .ToListAsync();
        }

        public async Task AddAsync(DispositionRecord entity)
        {
            await _context.DispositionRecords.AddAsync(entity);
        }

        public void Update(DispositionRecord entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.DispositionRecords.FindAsync(id);
            if (entity != null)
            {
                _context.DispositionRecords.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.DispositionRecords.AnyAsync(dr => dr.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}