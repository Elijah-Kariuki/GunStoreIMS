using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a shotgun with ATF eForms compliance.
    /// </summary>
    public sealed class Shotgun : LongGun
    {
        /// <summary>
        /// Initializes a new Shotgun, setting the FirearmType for ATF mapping.
        /// </summary>
        public Shotgun()
        {
            FirearmType = FirearmEnumType.Shotgun;
        }

        /// <summary>
        /// Performs shotgun-specific validations after the base (long gun) checks.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run base validations
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Non-NFA shotguns must meet minimum length requirements
            if (!IsNFAItem)
            {
                if (BarrelLengthInches < 18m)
                {
                    yield return new ValidationResult(
                        "Non-NFA shotguns must have a barrel length of at least 18 inches.",
                        new[] { nameof(BarrelLengthInches) }
                    );
                }
                if (OverallLengthInches < 26m)
                {
                    yield return new ValidationResult(
                        "Non-NFA shotguns must have an overall length of at least 26 inches.",
                        new[] { nameof(OverallLengthInches) }
                    );
                }
            }
        }
    }
}
