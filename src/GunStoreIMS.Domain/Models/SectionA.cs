using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Linq;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents Section A (Firearm(s) Description) of ATF Form 4473.
    /// Aligns with ATF Form 4473 (Aug 2023).
    /// </summary>
    public class SectionA : IValidatableObject
    {
        /// <summary>
        /// Q1-5: List of firearms (1-29). Stored in Form4473Record.Form4473FirearmLines, mapped here for serialization.
        /// </summary>
        [Required(ErrorMessage = "At least one firearm must be listed.")]
        [JsonPropertyName("Firearms")]
        [MinLength(1, ErrorMessage = "Must list at least one firearm.")]
        [MaxLength(29, ErrorMessage = "Cannot list more than 29 firearms.")]
        public List<Form4473FirearmLine> Firearms { get; set; } = new();

        /// <summary>
        /// Q6: Total number of firearms (spelled out).
        /// </summary>
        [Required(ErrorMessage = "Total Number (spelled out) is required.")]
        [StringLength(50)]
        [JsonPropertyName("TotalNumber")]
        public string TotalNumber { get; set; } = string.Empty;

        /// <summary>
        /// Q7: Is this a pawn redemption?
        /// </summary>
        [JsonPropertyName("IsPawnRedemption")]
        public bool IsPawnRedemption { get; set; }

        /// <summary>
        /// Q7: Line numbers for pawn redemptions. Required if IsPawnRedemption is true.
        /// </summary>
        [StringLength(100)]
        [JsonPropertyName("PawnRedemptionLines")]
        [RegularExpression(@"^(\d+)(,\d+)*$", ErrorMessage = "Pawn Redemption Lines must be comma-separated numbers.")]
        public string? PawnRedemptionLines { get; set; }

        /// <summary>
        /// Q8: Is this a private party transfer?
        /// </summary>
        [JsonPropertyName("IsPrivatePartyTransfer")]
        public bool IsPrivatePartyTransfer { get; set; }

        /// <summary>
        /// Indicates if a Continuation Sheet is used.
        /// Must be true if Firearms.Count >= 6.
        /// </summary>
        [JsonPropertyName("ContinuationSheet")]
        public bool? ContinuationSheet { get; set; }

        /// <summary>
        /// Performs validation checks for conditional logic.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validate ContinuationSheet must be true if 6+ firearms are listed
            if (Firearms.Count >= 6 && (ContinuationSheet == null || ContinuationSheet != true))
            {
                yield return new ValidationResult(
                    "ContinuationSheet must be true when 6+ firearms are listed.",
                    new[] { nameof(ContinuationSheet) }
                );
            }

            // Validate PawnRedemptionLines must be present when IsPawnRedemption is true
            if (IsPawnRedemption && string.IsNullOrWhiteSpace(PawnRedemptionLines))
            {
                yield return new ValidationResult(
                    "Pawn Redemption Lines must be provided when IsPawnRedemption is true.",
                    new[] { nameof(PawnRedemptionLines) }
                );
            }
        }
    }
}