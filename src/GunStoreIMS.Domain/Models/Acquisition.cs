using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunStoreIMS.Domain.Enums;
using GunStoreIMS.Domain.ValidationAttributes;

namespace GunStoreIMS.Domain.Models
{
    [RequireFflOrAddress(
        nameof(TransferorFFLNumber),
        nameof(TransferorAddressLine1),
        nameof(TransferorCity),
        nameof(TransferorState),
        nameof(TransferorZip),
        ErrorMessage = "Either the Transferor FFL# or full address must be provided."
    )]
    public class Acquisition : IValidatableObject
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        public Guid FirearmId { get; init; }

        [ForeignKey(nameof(FirearmId))]
        public virtual Firearm Firearm { get; init; } = default!;

        [Required]
        public DateTime DateUtc { get; set; }

        // Transferor
        [Required, StringLength(150)]
        public string TransferorName { get; set; } = default!;

        [StringLength(20)]
        public string? TransferorFFLNumber { get; set; }

        [StringLength(150)] public string? TransferorAddressLine1 { get; set; }
        [StringLength(150)] public string? TransferorAddressLine2 { get; set; }
        [StringLength(100)] public string? TransferorCity { get; set; }
        public USState? TransferorState { get; set; }
        [StringLength(15)] public string? TransferorZip { get; set; }

        // Concurrency
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            // no more FFL/address check here—
            // attribute covers that
            yield break;
        }
    }
}
