using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a firearm frame in compliance with ATF eForms regulations.
    /// </summary>
    public sealed class Frame : Firearm
    {
        /// <summary>
        /// Initializes a new Frame instance and ensures ATF classification alignment.
        /// </summary>
        public Frame()
        {
            FirearmType = FirearmEnumType.Frame;
        }

        /// <summary>
        /// Performs Frame-specific validations based on ATF guidelines.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // First, run all the base-class (Firearm) validations
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Frame must have a serial number
            if (string.IsNullOrWhiteSpace(SerialNumber))
            {
                yield return new ValidationResult(
                    "Frames must have a serial number.",
                    new[] { nameof(SerialNumber) }
                );
            }

            // Manufacturer name is required
            if (string.IsNullOrWhiteSpace(Manufacturer))
            {
                yield return new ValidationResult(
                    "Manufacturer name is required for frames.",
                    new[] { nameof(Manufacturer) }
                );
            }

            // If marked as privately made, FFL marking must be present
            if (IsPrivatelyMadeFirearm && string.IsNullOrWhiteSpace(YourFFLMarking))
            {
                yield return new ValidationResult(
                    "FFL marking is required for privately made frames.",
                    new[] { nameof(YourFFLMarking) }
                );
            }

            // If categorized as 'Other,' a description is mandatory
            if (FirearmType == FirearmEnumType.Other && string.IsNullOrWhiteSpace(OtherTypeDescription))
            {
                yield return new ValidationResult(
                    "A description is required when firearm type is 'Other'.",
                    new[] { nameof(OtherTypeDescription) }
                );
            }
        }
    }
}
