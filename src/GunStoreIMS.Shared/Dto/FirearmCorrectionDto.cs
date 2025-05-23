using System;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{
    public class FirearmCorrectionDto // DTO for making corrections to a Firearm record
    {
        // Include only fields that are permissible to correct, plus metadata for the correction
        public Guid FirearmId { get; set; } // ID of the firearm being corrected

        // Example correctable fields:
        public string? Model { get; set; }
        public string? CaliberOrGauge { get; set; } // If correcting caliber
        public decimal? BarrelLength { get; set; }
        public decimal? OverallLength { get; set; }
        public string? AdditionalMarkings { get; set; }
        public string? OtherTypeDescription { get; set; }
        // ... other fields that might have typos or initial misidentification

        [Required(ErrorMessage = "Reason for correction is mandatory.")]
        [StringLength(500)]
        public string ReasonForCorrection { get; set; } = default!;

        // Optionally, who made the correction and when (though server can log this)
        // public string CorrectedByUserId { get; set; }
        // public DateTime CorrectionDate { get; set; } = DateTime.UtcNow;
    }
}
