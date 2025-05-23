// GunStoreIMS.Application.Interfaces.IAcquisitionRecordService.cs
using GunStoreIMS.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GunStoreIMS.Domain.Interfaces
{
    public interface IAcquisitionRecordService
    {
        Task<IEnumerable<AcquisitionRecordDto>> GetAllAsync();
        Task<AcquisitionRecordDto?> GetByIdAsync(Guid id);
        Task<AcquisitionRecordDto> CreateAsync(AcquisitionRecordDto dto);
        Task<bool> UpdateAsync(Guid id, AcquisitionRecordDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
