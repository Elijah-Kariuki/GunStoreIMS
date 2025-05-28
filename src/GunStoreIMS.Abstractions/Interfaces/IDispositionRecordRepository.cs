// File: GunStoreIMS.Abstractions/Interfaces/IDispositionRecordRepository.cs
using GunStoreIMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GunStoreIMS.Abstractions.Interfaces
{
    public interface IDispositionRecordRepository
    {
        Task<DispositionRecord?> GetByIdAsync(Guid id);
        Task<IEnumerable<DispositionRecord>> GetAllAsync();
        Task AddAsync(DispositionRecord entity);
        void Update(DispositionRecord entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<int> SaveChangesAsync();
    }
}