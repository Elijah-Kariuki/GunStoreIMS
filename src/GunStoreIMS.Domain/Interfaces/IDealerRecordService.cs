using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.Domain.Interfaces
{
    public interface IDealerRecordService
    {
        Task<DealerRecord?> GetDealerRecordByIdAsync(Guid id);
        Task<IEnumerable<DealerRecord>> GetAllDealerRecordsAsync();
        Task<IEnumerable<DealerRecord>> GetRecordsByTradeNameAsync(string tradeName);
        Task<IEnumerable<DealerRecord>> GetRecordsByFflNumberAsync(string fflNumber);
        Task AddDealerRecordAsync(DealerRecord dealerRecord);
        Task<bool> UpdateDealerRecordAsync(DealerRecord dealerRecord);
        Task<bool> DeleteDealerRecordAsync(Guid id);
    }
}