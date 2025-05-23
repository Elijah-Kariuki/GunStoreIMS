// IFirearmService.cs
using GunStoreIMS.Domain.Utilities;
using GunStoreIMS.Shared.Dto;
using System;
using System.Threading.Tasks;

namespace GunStoreIMS.Application.Services
{
    public interface IFirearmService
    {
        Task<PagedResultDto<FirearmLineDto>> QueryFirearmsAsync(string status, int page, int pageSize);
        Task<FirearmDetailDto?> GetByIdAsync(Guid id);
        Task<OperationResult<FirearmDetailDto>> AcquireAsync(AcquisitionRecordDto dto);
        Task<bool> CorrectAsync(FirearmCorrectionDto dto);
        Task<OperationResult<FirearmDetailDto>> DisposeAsync(DispositionRecordDto dto);
        Task<OperationResult> ArchiveAsync(Guid id);
    }
}
