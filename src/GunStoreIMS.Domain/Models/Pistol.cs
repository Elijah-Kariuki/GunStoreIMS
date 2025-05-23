using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;


namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a pistol (handgun) with ATF eForms compliance.
    /// </summary>
    public sealed class Pistol : Handgun
    {
        /// <summary>
        /// Initializes a new Pistol, setting the FirearmType for ATF mapping.
        /// </summary>
        public Pistol()
        {
            FirearmType = FirearmEnumType.Pistol;
        }

        /// <summary>
        /// Performs pistol-specific validations after the base (handgun) checks.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run base validations
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Ensure a minimal barrel length typical for pistols (e.g., 3 inches)
            if (BarrelLengthInches < 3m)
            {
                yield return new ValidationResult(
                    "Pistols must have a barrel length of at least 3 inches.",
                    new[] { nameof(BarrelLengthInches) }
                );
            }
        }
    }
}
