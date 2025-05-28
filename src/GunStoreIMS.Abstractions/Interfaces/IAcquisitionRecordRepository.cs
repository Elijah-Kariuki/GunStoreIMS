// File: GunStoreIMS.Abstractions/Interfaces/IAcquisitionRecordRepository.cs
using GunStoreIMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GunStoreIMS.Abstractions.Interfaces
{
    public interface IAcquisitionRecordRepository
    {
        Task<AcquisitionRecord?> GetByIdAsync(Guid id);
        Task<IEnumerable<AcquisitionRecord>> GetAllAsync();
        Task AddAsync(AcquisitionRecord entity);
        void Update(AcquisitionRecord entity); // EF Core tracks changes, explicit Update sometimes not needed if entity is tracked
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<int> SaveChangesAsync();
        Task<AcquisitionRecord?> FindBySerialNumberAsync(string serialNumber); // From your controller logic
    }
}