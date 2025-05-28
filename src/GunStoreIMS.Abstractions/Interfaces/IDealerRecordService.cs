using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.Abstractions.Interfaces
{
    public interface IDealerRecordService
    {
        Task<DealerRecord?> GetDealerRecordByIdAsync(Guid id);
        Task<IEnumerable<DealerRecord>> GetAllDealerRecordsAsync(bool? isActive = null);
        Task<IEnumerable<DealerRecord>> GetRecordsByTradeNameAsync(string tradeName);
        Task<IEnumerable<DealerRecord>> GetRecordsByFflNumberAsync(string fflNumber);
        Task<(DealerRecord? Result, string? Error)> AddDealerRecordAsync(DealerRecord dealerRecord);
        Task<(bool Success, string? Error)> UpdateDealerRecordAsync(DealerRecord dealerRecord);
        Task<(bool Success, string? Error)> ArchiveDealerRecordAsync(Guid id);
        Task<(bool Success, string? Error)> ActivateDealerRecordAsync(Guid id);
        Task<bool> DeleteDealerRecordAsync(Guid id);
    }
}