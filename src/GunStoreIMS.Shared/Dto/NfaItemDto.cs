using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{
    public class NfaItemDto : FirearmDetailDto
    {
        [Required, StringLength(30)]
        public string NfaRegistryId { get; set; } = default!;

        [Required]
        public DateTime NfaRegistrationDateUtc { get; set; }

        [Required, StringLength(10)]
        public string RegistrationFormType { get; set; } = default!;

        [StringLength(40)]
        public string? TaxStampNumber { get; set; }
    }
}
