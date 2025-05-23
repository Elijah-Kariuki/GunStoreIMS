// GunStoreIMS.Domain.Models.SerialNumberHistory.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

// using System.Reflection.Metadata; // This was likely unused, can be removed if not needed for other parts.
using GunStoreIMS.Shared.Enums; // Assuming SerialChangeReason is in this namespace

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Tracks any change or correction to the firearm’s serial number
    /// (ATF Ruling 79‑7; bound‑book accuracy).
    /// </summary>
    public class SerialNumberHistory : IValidatableObject
    {
        [Key]
        public Guid Id { get; set; }

        // --- Link back to the firearm ---
        [Required]
        public Guid FirearmId { get; set; } // Foreign Key

        [ForeignKey(nameof(FirearmId))]
        public virtual Firearm Firearm { get; set; } = default!; // Navigation property

        // --- Change details ---
        [Required]
        [StringLength(50)] // Max length should match Firearm.SerialNumber
        public string PreviousSerial { get; set; } = default!;

        [Required]
        [StringLength(50)] // Max length should match Firearm.SerialNumber
        public string NewSerial { get; set; } = default!;

        [Required]
        public DateTime ChangeDateUtc { get; set; }

        [Required]
        public SerialChangeReason Reason { get; set; }

        /// <summary>Optional supporting document (variance letter, etc.).</summary>
        public Guid? DocumentId { get; set; } // Foreign Key to a Document entity (if you have one)

        [ForeignKey(nameof(DocumentId))]
        public virtual Document? Document { get; set; } // Navigation property (Assumes Document.cs exists)

        [StringLength(300)]
        public string? Notes { get; set; }

        // --- Validation ---
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PreviousSerial.Equals(NewSerial, StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("New serial must differ from previous serial.", new[] { nameof(NewSerial) });
            }

            if (string.IsNullOrWhiteSpace(NewSerial) || NewSerial.Length < 1) // Adjusted to match firearm serial length rules
            {
                yield return new ValidationResult("New serial number must be at least 1 character long.", new[] { nameof(NewSerial) });
            }
            // Add other validations as necessary, e.g., ChangeDateUtc not in future
        }

        // Helper for validation results (optional, can inline if preferred)
        private static ValidationResult Fail(string member, string msg) => new(msg, new[] { member });
    }
}
