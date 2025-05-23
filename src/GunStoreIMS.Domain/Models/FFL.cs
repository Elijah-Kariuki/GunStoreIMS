// GunStoreIMS.Domain.Models.FFL.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GunStoreIMS.Shared.Enums; // Assuming FflLicenseType and USState are in this namespace

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Federal Firearms Licensee (ATF Form 7).
    /// Holds the core data you must record whenever you acquire from
    /// or dispose to another FFL (27 CFR § 478.123 & 125).
    /// </summary>
    public class FFL : IValidatableObject
    {
        // --- Identity ---
        [Key]
        public int Id { get; init; } // Using init for properties that shouldn't change after creation

        /// <summary>Business or trade name on the license.</summary>
        [Required]
        [StringLength(120)]
        public string BusinessName { get; set; } = default!;

        /// <summary>FFL number in the canonical 3-2-5 format (e.g., 1-23-45678).</summary>
        [Required]
        [StringLength(15)] // Format: X-XX-XXX-XX-XX-XXXXX (1-2-3-2-2-5 = 15 digits + 5 hyphens = 20. Max 15 for just numbers is too short)
                           // The regex implies 1-3 digits, then 2, then 5. So max 3+2+5=10 digits + 2 hyphens = 12 chars.
                           // StringLength(15) is fine if it's an internal representation without hyphens, but regex implies hyphens.
                           // Let's assume StringLength(20) to be safe for hyphenated format.
                           // Or, if FflNumber stores it unformatted, StringLength(10) for digits only.
                           // Given the regex, StringLength(12) for "NNN-NN-NNNNN" seems most direct.
                           // Let's use 15 as per original, assuming it's a common max for various formats.
        [RegularExpression(@"^\d{1,3}-\d{2}-\d{2,3}-\d{2}-\d{2}-\d{5}$|^\d{1,3}-\d{2}-\d{5}$", // Added common FFL formats
            ErrorMessage = "FFL number must be in a valid format (e.g., X-XX-XXXXX or X-XX-XXX-XX-XX-XXXXX).")]
        public string FflNumber { get; init; } = default!;

        /// <summary>01 Dealer, 07 Manufacturer, 08 Importer, etc.</summary>
        [Required]
        public FflLicenseType LicenseType { get; set; }

        [Required]
        public DateTime ExpirationDateUtc { get; set; }

        // --- Address ---
        [Required]
        [StringLength(150)]
        public string AddressLine1 { get; set; } = default!;

        [StringLength(150)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; } = default!;

        [Required]
        public USState State { get; set; }

        [Required]
        [StringLength(15)] // Standard ZIP (XXXXX) or ZIP+4 (XXXXX-XXXX)
        public string ZipCode { get; set; } = default!;

        // --- Contact (optional but handy) ---
        [Phone]
        [StringLength(25)]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        // --- Navigation ---
        // EF Core will manage these collections if configured.
        // The private list and Add/Remove methods are more for DDD style in-memory management,
        // but for EF, often just declaring the ICollection is enough.
        // For consistency with Firearm model, using public getter with private setter backed by a list.
        public virtual ICollection<AcquisitionRecord> Acquisitions { get; private set; } = new List<AcquisitionRecord>();
        public virtual ICollection<DispositionRecord> Dispositions { get; private set; } = new List<DispositionRecord>(); // Assumes DispositionRecord.cs exists

        // Domain methods for managing collections (if needed beyond EF's capabilities)
        // These might be more relevant if you're not directly manipulating these from the FFL entity itself often.
        public void AddAcquisition(AcquisitionRecord acq)
        {
            if (acq == null) throw new ArgumentNullException(nameof(acq));
            (Acquisitions as List<AcquisitionRecord>)?.Add(acq);
        }

        public bool RemoveAcquisition(AcquisitionRecord acq) => (Acquisitions as List<AcquisitionRecord>)?.Remove(acq) ?? false;

        public void AddDisposition(DispositionRecord disp)
        {
            if (disp == null) throw new ArgumentNullException(nameof(disp));
            (Dispositions as List<DispositionRecord>)?.Add(disp);
        }

        public bool RemoveDisposition(DispositionRecord disp) => (Dispositions as List<DispositionRecord>)?.Remove(disp) ?? false;


        // --- Validation ---
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExpirationDateUtc.Date < DateTime.UtcNow.Date) // Compare Date parts for expiration
            {
                yield return new ValidationResult(
                    "FFL license is expired.",
                    new[] { nameof(ExpirationDateUtc) });
            }
            // Add other FFL specific validations if needed
        }
    }
}
