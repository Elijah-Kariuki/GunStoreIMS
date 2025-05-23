using GunStoreIMS.Domain.Models;
using GunStoreIMS.Domain.Utilities;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.BusinessRules
{
    public class FirearmArchivingRules
    {
        public OperationResult CanArchive(Firearm firearm)
        {
            if (firearm == null)
                return OperationResult.Fail("Firearm not found.");

            // Ensure firearm is not currently in active inventory
            if (firearm.CurrentStatus != FirearmStatus.Disposed)
                return OperationResult.Fail("Only disposed firearms can be archived.");

            return OperationResult.Success();
        }
    }
}
