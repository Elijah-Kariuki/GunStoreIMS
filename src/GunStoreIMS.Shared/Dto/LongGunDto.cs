using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// Data Transfer Object for LongGun entities (e.g., Rifle, Shotgun).
    /// Mirrors the domain model’s barrel and overall length properties.
    /// </summary>
    public class LongGunDto : FirearmDetailDto
    {
        /// <summary>
        /// Barrel length in inches (10–60).
        /// </summary>
        [Range(10, 60, ErrorMessage = "Barrel length must be between 10 and 60 inches.")]
        public decimal BarrelLengthInches { get; set; }

        /// <summary>
        /// Overall length in inches (20–120).
        /// </summary>
        [Range(20, 120, ErrorMessage = "Overall length must be between 20 and 120 inches.")]
        public decimal OverallLengthInches { get; set; }
    }
}
