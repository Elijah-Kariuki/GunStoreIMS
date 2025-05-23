using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a revolver (handgun) with ATF eForms compliance.
    /// </summary>
    public sealed class Revolver : Handgun
    {
        /// <summary>
        /// Initializes a new Revolver, setting the FirearmType for ATF mapping.
        /// </summary>
        public Revolver()
        {
            FirearmType = FirearmEnumType.Revolver;
        }

        /// <summary>
        /// Performs revolver-specific validations after the base (handgun) checks.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run base validations
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Ensure a minimal barrel length typical for revolvers (e.g., 2.5 inches)
            if (BarrelLengthInches < 2.5m)
            {
                yield return new ValidationResult(
                    "Revolvers must have a barrel length of at least 2.5 inches.",
                    new[] { nameof(BarrelLengthInches) }
                );
            }
        }
    }
}
