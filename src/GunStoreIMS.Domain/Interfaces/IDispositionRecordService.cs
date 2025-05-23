// GunStoreIMS.Application.Interfaces.IDispositionRecordService.cs
using GunStoreIMS.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GunStoreIMS.Domain.Interfaces
{
    public interface IDispositionRecordService
    {
        Task<IEnumerable<DispositionRecordDto>> GetAllAsync();
        Task<DispositionRecordDto?> GetByIdAsync(Guid id);
        Task<DispositionRecordDto> CreateAsync(DispositionRecordDto dto);
        Task<bool> UpdateAsync(Guid id, DispositionRecordDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}