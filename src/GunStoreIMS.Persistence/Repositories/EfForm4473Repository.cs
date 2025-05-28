// GunStoreIMS.Persistence.Repositories.Form4473Repository.cs (New File)
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
    public class Form4473Repository : IForm4473Repository
    {
        private readonly FirearmsInventoryDB _context;

        public Form4473Repository(FirearmsInventoryDB context)
        {
            _context = context;
        }

        public async Task<Form4473Record?> GetByIdAsync(Guid id) =>
            await _context.Form4473Records
                .Include(r => r.Form4473FirearmLines)
                // Eagerly load other owned types if they aren't automatically included
                // .Include(r => r.BuyerInfoSnapshot.ResidenceAddress) // Example if deeply nested
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Form4473Record>> GetAllAsync() =>
            await _context.Form4473Records
                .Include(r => r.Form4473FirearmLines)
                .ToListAsync();

        public async Task AddAsync(Form4473Record record) =>
            await _context.Form4473Records.AddAsync(record);

        public Task UpdateAsync(Form4473Record record)
        {
            _context.Entry(record).State = EntityState.Modified;
            // Also mark owned types as modified if necessary, though EF Core often handles this.
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var record = await GetByIdAsync(id);
            if (record != null) _context.Form4473Records.Remove(record);
        }

        public async Task<bool> ExistsAsync(Guid id) =>
            await _context.Form4473Records.AnyAsync(e => e.Id == id);

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}