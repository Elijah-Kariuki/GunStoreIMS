using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunStoreIMS.Shared.Enums;
using GunStoreIMS.Shared.Validation;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents the disposition of a firearm, ensuring ATF compliance and proper record-keeping.
    /// </summary>
    [RequireFflOrAddress(
        nameof(TransfereeFFLNumber),
        nameof(TransfereeAddressLine1),
        nameof(TransfereeCity),
        nameof(TransfereeState),
        nameof(TransfereeZip),
        ErrorMessage = "Either the Transferee FFL# or full Transferee address (Line 1, City, State, Zip) must be provided."
    )]
    public class DispositionRecord : IValidatableObject
    {
        /// <summary>
        /// Unique identifier for the disposition record.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Date the firearm was transferred.
        /// </summary>
        [Required(ErrorMessage = "Transaction date is required.")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Type of disposition (e.g., Transfer, Sale, Destroyed).
        /// </summary>
        [Required(ErrorMessage = "Disposition type is required.")]
        [StringLength(50)]
        public string DispositionType { get; set; } = default!;

        /// <summary>
        /// Name of the transferee (buyer, dealer, or receiving entity).
        /// </summary>
        [Required(ErrorMessage = "Transferee name is required.")]
        [StringLength(200)]
        public string TransfereeName { get; set; } = default!;

        /// <summary>
        /// FFL number of the transferee, if applicable.
        /// </summary>
        [StringLength(30, ErrorMessage = "Transferee FFL number cannot exceed 30 characters.")]
        [RegularExpression(@"^\d{1,2}-\d{2,3}-\d{3}-\d{2}-\d{2}-\d{5}$", ErrorMessage = "Invalid FFL number format.")]
        public string? TransfereeFFLNumber { get; set; }

        /// <summary>
        /// Full address of the transferee (required for non-FFL dispositions).
        /// </summary>
        [StringLength(150)] public string? TransfereeAddressLine1 { get; set; }
        [StringLength(150)] public string? TransfereeAddressLine2 { get; set; }
        [StringLength(100)] public string? TransfereeCity { get; set; }
        public USState? TransfereeState { get; set; }
        [StringLength(15, ErrorMessage = "Transferee zip code cannot exceed 15 characters.")]
        public string? TransfereeZip { get; set; }

        /// <summary>
        /// ID of the firearm being disposed of.
        /// </summary>
        [Required(ErrorMessage = "Firearm ID is required.")]
        public Guid FirearmId { get; set; }

        [ForeignKey(nameof(FirearmId))]
        public virtual Firearm Firearm { get; set; } = default!;

        /// <summary>
        /// Serial number of the firearm in the disposition record.
        /// </summary>
        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9.\-/]+$", ErrorMessage = "Invalid characters in serial number.")]
        public string SerialNumber { get; set; } = default!;

        /// <summary>
        /// Form 4473 transaction number or NICS NTN for non-FFL transferee.
        /// </summary>
        [StringLength(50)]
        public string? Form4473TransactionNumber { get; set; }

        /// <summary>
        /// Additional notes regarding the disposition.
        /// </summary>
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; }

        /// <summary>
        /// Concurrency control for database integrity.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        /// <summary>
        /// Custom validation for ATF compliance.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TransactionDate > DateTime.UtcNow)
                yield return new ValidationResult("Transaction date cannot be in the future.", new[] { nameof(TransactionDate) });

            if (string.IsNullOrWhiteSpace(TransfereeFFLNumber) && string.IsNullOrWhiteSpace(Form4473TransactionNumber))
            {
                yield return new ValidationResult(
                    "Form 4473 Transaction Number (or NICS NTN) is required for dispositions to non-FFL transferees.",
                    new[] { nameof(Form4473TransactionNumber) }
                );
            }
        }
    }
}
