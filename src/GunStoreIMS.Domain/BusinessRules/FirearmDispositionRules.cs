using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Utilities;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.BusinessRules
{
    public class FirearmDispositionRules
    {
        public OperationResult CanDispose(DispositionRecordDto dto, Firearm firearm)
        {
            if (dto == null || firearm == null)
                return OperationResult.Fail("Invalid input data.");

            // Ensure firearm is not already disposed
            if (firearm.CurrentStatus == FirearmStatus.Disposed)
                return OperationResult.Fail("This firearm has already been disposed.");

            // Additional disposition logic here (e.g., buyer background checks)
            return OperationResult.Success();
        }
    }
}
