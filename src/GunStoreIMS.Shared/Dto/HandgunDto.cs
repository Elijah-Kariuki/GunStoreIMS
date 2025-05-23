using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{
    public class HandgunDto : FirearmDetailDto
    {
        [Range(1, 20)]
        public decimal BarrelLengthInches { get; set; }

        // Provide both the string name and the numeric CaliberId
        public int CaliberId { get; set; }
        public string CaliberName { get; set; } = default!;

        public bool IsNFAItem { get; set; }
        public string? NfaClass { get; set; }

        public string OwningFflName { get; set; } = default!;
    }
}
