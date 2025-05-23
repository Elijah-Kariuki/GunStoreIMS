using GunStoreIMS.Domain.Models;
using GunStoreIMS.Domain.Utilities;
using GunStoreIMS.Shared.Dto;

namespace GunStoreIMS.Domain.BusinessRules
{
    /// <summary>
    /// Centralized manager for firearm business rule validations.
    /// </summary>
    public class FirearmBusinessRulesManager
    {
        private readonly FirearmDispositionRules _dispositionRules;
        private readonly FirearmArchivingRules _archivingRules;

        public FirearmBusinessRulesManager()
        {
            _dispositionRules = new FirearmDispositionRules();
            _archivingRules = new FirearmArchivingRules();
        }

        /// <summary>
        /// Determines if a firearm can be disposed.
        /// </summary>
        public OperationResult CanDispose(DispositionRecordDto dto, Firearm firearm)
            => _dispositionRules.CanDispose(dto, firearm);

        /// <summary>
        /// Determines if a firearm can be archived.
        /// </summary>
        public OperationResult CanArchive(Firearm firearm)
            => _archivingRules.CanArchive(firearm);
    }
}
