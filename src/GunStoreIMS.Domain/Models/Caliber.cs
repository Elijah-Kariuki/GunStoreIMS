using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    public class Caliber : IValidatableObject
    {
        [Key]
        public int Id { get; init; }

        [Required, StringLength(50)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Standardized code (e.g. “9MM”, “.45ACP”), 1–5 alphanumerics.
        /// Not used for gauge definitions like “12 gauge”.
        /// </summary>
        [StringLength(5)]
        [RegularExpression(@"^[A-Z0-9]{1,5}$",
            ErrorMessage = "StandardCode must be 1–5 uppercase alphanumeric characters.")]
        public string? StandardCode { get; set; }

        public IReadOnlyCollection<Firearm> Firearms => _firearms;
        private readonly List<Firearm> _firearms = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            bool isGauge = Name.IndexOf("gauge", StringComparison.OrdinalIgnoreCase) >= 0;

            if (isGauge && !string.IsNullOrEmpty(StandardCode))
            {
                yield return new ValidationResult(
                    "Gauge calibers must not have a StandardCode.",
                    new[] { nameof(StandardCode) }
                );
            }

            if (!isGauge && string.IsNullOrWhiteSpace(StandardCode))
            {
                yield return new ValidationResult(
                    "Non-gauge calibers require a StandardCode.",
                    new[] { nameof(StandardCode) }
                );
            }
        }
    }
}
