using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a rifle with ATF eForms compliance.
    /// </summary>
    public sealed class Rifle : LongGun
    {
        /// <summary>
        /// Initializes a new Rifle, setting the FirearmType for ATF mapping.
        /// </summary>
        public Rifle() => FirearmType = FirearmEnumType.Rifle;

        /// <summary>
        /// Performs rifle-specific validations after the base (long gun) checks.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run base validations
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Non-NFA rifles must meet minimum length requirements
            if (!IsNFAItem)
            {
                if (BarrelLengthInches < 16m)
                {
                    yield return new ValidationResult(
                        "Non-NFA rifles must have a barrel length of at least 16 inches.",
                        new[] { nameof(BarrelLengthInches) }
                    );
                }

                if (OverallLengthInches < 26m)
                {
                    yield return new ValidationResult(
                        "Non-NFA rifles must have an overall length of at least 26 inches.",
                        new[] { nameof(OverallLengthInches) }
                    );
                }
            }
        }
    }
}
