// GunStoreIMS.Abstractions.Interfaces.IForm4473Repository.cs
using GunStoreIMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GunStoreIMS.Abstractions.Interfaces
{
    public interface IForm4473Repository
    {
        Task<Form4473Record?> GetByIdAsync(Guid id);
        Task<IEnumerable<Form4473Record>> GetAllAsync(); // Consider pagination
        Task AddAsync(Form4473Record record);
        Task UpdateAsync(Form4473Record record);
        Task DeleteAsync(Guid id); // Be cautious with ATF record retention rules
        Task<bool> ExistsAsync(Guid id);
        Task<int> SaveChangesAsync();
    }
}