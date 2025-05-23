using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a firearm receiver in compliance with ATF eForms regulations.
    /// </summary>
    public sealed class Receiver : Firearm
    {
        /// <summary>
        /// Initializes a new Receiver instance and ensures ATF classification alignment.
        /// </summary>
        public Receiver()
        {
            FirearmType = FirearmEnumType.Receiver;
        }

        /// <summary>
        /// Performs Receiver-specific validations based on ATF guidelines.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run base validations
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Receiver must have a serial number
            if (string.IsNullOrWhiteSpace(SerialNumber))
            {
                yield return new ValidationResult(
                    "Receivers must have a serial number.",
                    new[] { nameof(SerialNumber) }
                );
            }

            // Manufacturer name is required
            if (string.IsNullOrWhiteSpace(Manufacturer))
            {
                yield return new ValidationResult(
                    "Manufacturer name is required for receivers.",
                    new[] { nameof(Manufacturer) }
                );
            }

            // If marked as privately made, FFL marking must be present
            if (IsPrivatelyMadeFirearm && string.IsNullOrWhiteSpace(YourFFLMarking))
            {
                yield return new ValidationResult(
                    "FFL marking is required for privately made receivers.",
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
