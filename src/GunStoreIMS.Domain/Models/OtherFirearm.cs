using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents any 'Other' type firearm in compliance with ATF eForms regulations.
    /// </summary>
    public sealed class OtherFirearm : Firearm
    {
        /// <summary>
        /// Initializes a new OtherFirearm instance and ensures ATF classification alignment.
        /// </summary>
        public OtherFirearm()
        {
            FirearmType = FirearmEnumType.Other;
        }

        /// <summary>
        /// Performs OtherFirearm-specific validations based on ATF guidelines.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run base validations first
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Ensure a description is provided for 'Other' types
            if (string.IsNullOrWhiteSpace(OtherTypeDescription))
            {
                yield return new ValidationResult(
                    "A description is required when firearm type is 'Other'.",
                    new[] { nameof(OtherTypeDescription) }
                );
            }

            // Manufacturer name is required (redundant with base [Required], but safe to reinforce)
            if (string.IsNullOrWhiteSpace(Manufacturer))
            {
                yield return new ValidationResult(
                    "Manufacturer name is required for other firearm types.",
                    new[] { nameof(Manufacturer) }
                );
            }

            // Frames/Receivers have their own classes—ensure this isn't misused
            if (IsFrameOrReceiver)
            {
                yield return new ValidationResult(
                    "Use the Frame or Receiver class for those specific firearm types.",
                    new[] { nameof(FirearmType) }
                );
            }

            // If privately made, FFL marking must be present
            if (IsPrivatelyMadeFirearm && string.IsNullOrWhiteSpace(YourFFLMarking))
            {
                yield return new ValidationResult(
                    "FFL marking is required for privately made 'Other' firearms.",
                    new[] { nameof(YourFFLMarking) }
                );
            }
        }
    }
}
