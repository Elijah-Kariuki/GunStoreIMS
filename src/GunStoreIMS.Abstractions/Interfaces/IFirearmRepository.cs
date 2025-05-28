// File: GunStoreIMS.Abstractions/Interfaces/IFirearmRepository.cs
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Enums; // For FirearmStatus
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GunStoreIMS.Abstractions.Interfaces
{
    public interface IFirearmRepository
    {
        Task<Firearm?> GetByIdAsync(Guid id);
        Task<(IEnumerable<Firearm> Items, int TotalCount)> QueryAsync(FirearmStatus status, int page, int pageSize);
        Task AddAsync(Firearm entity);
        void Update(Firearm entity); // For marking as modified
        Task<bool> ExistsBySerialNumberAsync(string serialNumber, string manufacturer, string model, string? importerName = null); // For uniqueness checks
        Task<int> SaveChangesAsync();
        // Potentially other query methods as needed, e.g., FindBySerialNumber etc.
    }
}