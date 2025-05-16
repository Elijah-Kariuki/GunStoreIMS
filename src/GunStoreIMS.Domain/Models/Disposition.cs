using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunStoreIMS.Domain.Enums;
using GunStoreIMS.Domain.ValidationAttributes;

namespace GunStoreIMS.Domain.Models
{
    [RequireFflOrAddress(
        nameof(TransfereeFFLNumber),
        nameof(TransfereeAddressLine1),
        nameof(TransfereeCity),
        nameof(TransfereeState),
        nameof(TransfereeZip),
        ErrorMessage = "Either the Transferee FFL# or full address must be provided."
    )]
    public class Disposition : IValidatableObject
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        public Guid FirearmId { get; init; }

        [ForeignKey(nameof(FirearmId))]
        public virtual Firearm Firearm { get; init; } = default!;

        [Required]
        public DateTime DateUtc { get; set; }

        // Transferee
        [Required, StringLength(150)]
        public string TransfereeName { get; set; } = default!;

        [StringLength(20)]
        public string? TransfereeFFLNumber { get; set; }

        [StringLength(150)] public string? TransfereeAddressLine1 { get; set; }
        [StringLength(150)] public string? TransfereeAddressLine2 { get; set; }
        [StringLength(100)] public string? TransfereeCity { get; set; }
        public USState? TransfereeState { get; set; }
        [StringLength(15)] public string? TransfereeZip { get; set; }

        // Form 4473
        [StringLength(50)]
        public string? Form4473SerialNumber { get; set; }

        // Concurrency
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            // extra rule: non‐FFL transferee must supply Form4473SerialNumber
            if (string.IsNullOrWhiteSpace(TransfereeFFLNumber)
                && string.IsNullOrWhiteSpace(Form4473SerialNumber))
            {
                yield return new ValidationResult(
                    "Form 4473 Serial Number is required for dispositions to non-FFL transferees.",
                    new[] { nameof(Form4473SerialNumber) }
                );
            }
        }
    }
}
