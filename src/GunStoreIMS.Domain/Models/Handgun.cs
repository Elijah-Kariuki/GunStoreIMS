using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunStoreIMS.Shared.Enums;


namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Handgun specialization with ATF eForms compliance fields.
    /// </summary>
    public abstract class Handgun : Firearm
    {
        /// <summary>
        /// Aligns with eForm 4473 barrel length (in inches).
        /// Backed by base.BarrelLength.
        /// </summary>
        [NotMapped]
        [Range(1.0, 20.0, ErrorMessage = "Barrel length must be between 1 and 20 inches.")]
        public decimal BarrelLengthInches
        {
            get => BarrelLength ?? 0;
            set => BarrelLength = value;
        }

        /// <summary>
        /// eForm 4473 requires action type (e.g., "single action", "double action").
        /// </summary>
        [Required(ErrorMessage = "Action type is required for handguns.")]
        [StringLength(50, ErrorMessage = "Action type cannot exceed 50 characters.")]
        public string ActionType { get; set; } = default!;

        /// <summary>
        /// Magazine capacity must be disclosed on eForm 4473.
        /// </summary>
        [Required(ErrorMessage = "Magazine capacity is required.")]
        [Range(1, 100, ErrorMessage = "Magazine capacity must be between 1 and 100 rounds.")]
        public int MagazineCapacity { get; set; }

        /// <summary>
        /// Convenience property for double-action detection.
        /// </summary>
        [NotMapped]
        public bool IsDoubleAction => ActionType?.IndexOf("double", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Handguns must be either Pistol or Revolver.
        /// Includes ATF-specific validation.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Run base validations
            foreach (var result in base.Validate(validationContext))
                yield return result;

            // Ensure FirearmType matches handgun categories
            if (FirearmType != FirearmEnumType.Pistol && FirearmType != FirearmEnumType.Revolver)
            {
                yield return new ValidationResult(
                    "Handgun FirearmType must be either Pistol or Revolver.",
                    new[] { nameof(FirearmType) }
                );
            }

            // ActionType enforcement
            if (string.IsNullOrWhiteSpace(ActionType))
            {
                yield return new ValidationResult(
                    "Action type is required.",
                    new[] { nameof(ActionType) }
                );
            }

            // MagazineCapacity enforcement
            if (MagazineCapacity < 1 || MagazineCapacity > 100)
            {
                yield return new ValidationResult(
                    "Magazine capacity must be between 1 and 100 rounds.",
                    new[] { nameof(MagazineCapacity) }
                );
            }
        }
    }
}
