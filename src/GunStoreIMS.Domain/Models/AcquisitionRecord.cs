using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunStoreIMS.Shared.Enums;
using GunStoreIMS.Shared.Validation;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a record of firearm acquisition, ensuring ATF compliance and traceability.
    /// </summary>
    [RequireFflOrAddress(
        nameof(SourceLicenseNumber),
        nameof(SourceAddress),
        ErrorMessage = "Either the Source FFL# or the full Source address must be provided for an acquisition."
    )]
    public class AcquisitionRecord : IValidatableObject
    {
        /// <summary>
        /// Unique identifier for the acquisition record.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Date the firearm was acquired.
        /// </summary>
        [Required(ErrorMessage = "Acquisition date is required.")]
        [DataType(DataType.Date)]
        public DateTime AcquisitionDate { get; set; }

        /// <summary>
        /// FirearmType = global using FirearmEnumType = GunStoreIMS.Shared.Enums.FirearmType;
        /// </summary>
        [Required(ErrorMessage = "Firearm type is required.")]
        public FirearmType FirearmType { get; set; }

        /// <summary>
        /// Seller or dealer's name from whom the firearm was acquired.
        /// </summary>
        [Required(ErrorMessage = "Source name is required.")]
        [StringLength(200, ErrorMessage = "Source name cannot exceed 200 characters.")]
        public string SourceName { get; set; } = default!;

        // --- Source Details (Seller/FFL Information) ---

        /// <summary>
        /// Federal Firearms License (FFL) number of the source, if applicable.
        /// </summary>
        [StringLength(30, ErrorMessage = "Source license number cannot exceed 30 characters.")]
        [RegularExpression(@"^\d{1,2}-\d{2,3}-\d{3}-\d{2}-\d{2}-\d{5}$",
            ErrorMessage = "Invalid FFL number format.")]
        public string? SourceLicenseNumber { get; set; }

        /// <summary>
        /// Address of the source (required if source is a non-licensee).
        /// </summary>
        [StringLength(300, ErrorMessage = "Source address cannot exceed 300 characters.")]
        public string? SourceAddress { get; set; }

        // --- Firearm Association ---

        /// <summary>
        /// Firearm ID linked to this acquisition record.
        /// </summary>
        [Required(ErrorMessage = "Firearm ID is required.")]
        public Guid FirearmId { get; set; }

        [ForeignKey(nameof(FirearmId))]
        public virtual Firearm Firearm { get; set; } = default!;

        /// <summary>
        /// Serial number of the acquired firearm.
        /// </summary>
        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(100, ErrorMessage = "Serial number cannot exceed 100 characters.")]
        [RegularExpression(@"^[A-Za-z0-9.\-/]+$", ErrorMessage = "Invalid characters in serial number.")]
        public string SerialNumber { get; set; } = default!;

        /// <summary>
        /// Indicates if the acquisition is part of a private sale.
        /// </summary>
        public bool IsPrivateSale { get; set; }

        /// <summary>
        /// Any relevant acquisition notes (optional but useful for documentation).
        /// </summary>
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; }

        // --- Concurrency Control ---

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        // --- Validation Logic ---

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AcquisitionDate.Date > DateTime.UtcNow.Date)
                yield return new ValidationResult(
                    "Acquisition date cannot be in the future.",
                    new[] { nameof(AcquisitionDate) }
                );

            if (string.IsNullOrWhiteSpace(SerialNumber))
                yield return new ValidationResult(
                    "Serial number must be provided.",
                    new[] { nameof(SerialNumber) }
                );
        }
    }
}
