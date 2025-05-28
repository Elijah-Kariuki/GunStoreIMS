using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums; // Ensure this namespace contains FflLicenseType and USState

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a Federal Firearms Licensee (FFL).
    /// This class holds core FFL data, license details, and contact information.
    /// It serves as the primary entity for storing FFL information for A&D and Form 4473 references.
    /// </summary>
    public class DealerRecord : IValidatableObject
    {
        /// <summary>
        /// Primary key for the FFL record.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Business or trade name on the license.
        /// </summary>
        [Required(ErrorMessage = "Trade Name is required.")]
        [StringLength(120, ErrorMessage = "Trade Name cannot exceed 120 characters.")]
        public string TradeName { get; set; } = default!;

        /// <summary>
        /// FFL number.
        /// </summary>
        [Required(ErrorMessage = "FFL Number is required.")]
        [StringLength(20, ErrorMessage = "FFL Number cannot exceed 20 characters.")]
        [RegularExpression(@"^\d{1,3}-\d{2}-\d{2,3}-\d{2}-\d{2}-\d{5}$|^\d{1,3}-\d{2}-\d{5}$",
            ErrorMessage = "FFL number must be in a valid format (e.g., X-XX-XXXXX or long form).")]
        public string FFLNumber { get; set; } = default!;

        /// <summary>
        /// Type of FFL (e.g., 01 Dealer, 07 Manufacturer).
        /// </summary>
        [Required(ErrorMessage = "License Type is required.")]
        public FflLicenseType LicenseType { get; set; }

        /// <summary>
        /// Date the FFL expires (UTC).
        /// </summary>
        [Required(ErrorMessage = "License Expiration Date is required.")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDateUtc { get; set; }

        /// <summary>
        /// Primary street address line.
        /// </summary>
        [Required(ErrorMessage = "Address Line 1 is required.")]
        [StringLength(150, ErrorMessage = "Address Line 1 cannot exceed 150 characters.")]
        public string AddressLine1 { get; set; } = default!;

        /// <summary>
        /// Optional secondary address line.
        /// </summary>
        [StringLength(150, ErrorMessage = "Address Line 2 cannot exceed 150 characters.")]
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string City { get; set; } = default!;

        /// <summary>
        /// US State.
        /// </summary>
        [Required(ErrorMessage = "State is required.")]
        public USState State { get; set; }

        /// <summary>
        /// ZIP Code.
        /// </summary>
        [Required(ErrorMessage = "ZIP Code is required.")]
        [StringLength(15, ErrorMessage = "ZIP Code cannot exceed 15 characters.")]
        [RegularExpression(@"^\d{5}(?:-\d{4})?$", ErrorMessage = "Use 12345 or 12345-6789 format.")]
        public string ZipCode { get; set; } = default!;

        /// <summary>
        /// Optional contact phone number.
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(25, ErrorMessage = "Phone number cannot exceed 25 characters.")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Optional contact email address.
        /// </summary>
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        public string? Email { get; set; }

        /// <summary>
        /// Date this FFL's info was first recorded in the system.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime RecordDate { get; set; } = DateTime.UtcNow.Date; // Default to now

        /// <summary>
        /// Date this FFL record was last verified/updated in your system.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if this FFL record is active and can be used for new transactions.
        /// </summary>
        public bool IsActive { get; set; } = true; // Added for soft-delete/archiving

        /// <summary>
        /// Timestamp for concurrency control.
        /// </summary>
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // --- Navigation Properties ---
        public virtual ICollection<AcquisitionRecord> AcquisitionsFrom { get; private set; } = new List<AcquisitionRecord>();
        public virtual ICollection<DispositionRecord> DispositionsTo { get; private set; } = new List<DispositionRecord>();
        public virtual ICollection<Form4473Record> Form4473s { get; private set; } = new List<Form4473Record>();

        /// <summary>
        /// Validates the FFL record.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Note: You might want to allow *saving* an expired FFL but prevent *using* it in new transactions.
            // This validation prevents *saving* an expired license *if* it's marked active.
            if (IsActive && ExpirationDateUtc.Date < DateTime.UtcNow.Date)
            {
                yield return new ValidationResult(
                    "An active FFL license cannot be expired.",
                    new[] { nameof(ExpirationDateUtc), nameof(IsActive) });
            }
        }
    }
}