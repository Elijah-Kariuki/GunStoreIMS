using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a Short-Barreled Rifle (SBR) in compliance with ATF eForms regulations.
    /// </summary>
    public sealed class ShortBarreledRifleModel : Firearm
    {
        /// <summary>
        /// Initializes a new ShortBarreledRifleModel instance and sets its ATF classification.
        /// </summary>
        public ShortBarreledRifleModel()
        {
            FirearmType = FirearmEnumType.ShortBarreledRifle;
        }

        /// <summary>
        /// Performs SBR-specific validations (barrel length, overall length, etc.).
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run all base (Firearm) validations first
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Barrel length must be < 16" for an SBR
            if (!BarrelLength.HasValue || BarrelLength.Value >= 16m)
            {
                yield return new ValidationResult(
                    "A Short-Barreled Rifle must have a barrel length less than 16 inches.",
                    new[] { nameof(BarrelLength) }
                );
            }

            // Overall length is required
            if (!OverallLength.HasValue)
            {
                yield return new ValidationResult(
                    "Overall length is required for NFA firearms.",
                    new[] { nameof(OverallLength) }
                );
            }
        }
    }
}
