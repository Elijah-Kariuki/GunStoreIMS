using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionC : IValidatableObject
    {
        /// <summary>
        /// 24. Category of firearm(s) (Must have at least one unique entry)
        /// </summary>
        [Required(ErrorMessage = "Firearm category is required.")]
        [JsonPropertyName("FirearmCategory")]
        [MinLength(1, ErrorMessage = "At least one firearm category must be selected.")]
        public List<string> FirearmCategory { get; set; } = new();

        /// <summary>
        /// Gun Show/Event Details (Optional)
        /// </summary>
        [JsonPropertyName("GunShowDetails")]
        public GunShowDetails? GunShowDetails { get; set; }

        /// <summary>
        /// Identification Details (Required)
        /// </summary>
        [Required(ErrorMessage = "Identification details are required.")]
        [JsonPropertyName("IdentificationDetails")]
        public Identification IdentificationDetails { get; set; } = default!;

        /// <summary>
        /// 26.b. Supplemental Documentation (Max 200 characters)
        /// </summary>
        [StringLength(200, ErrorMessage = "Supplemental documentation cannot exceed 200 characters.")]
        [JsonPropertyName("SupplementalDocs")]
        public string? SupplementalDocs { get; set; }

        /// <summary>
        /// PCS Orders (Optional)
        /// </summary>
        [JsonPropertyName("PCSOrders")]
        public PCSOrders? PCSOrders { get; set; }

        /// <summary>
        /// 26.d. Exception to Nonimmigrant Alien Prohibition (Max 200 characters)
        /// </summary>
        [StringLength(200, ErrorMessage = "Exception documentation cannot exceed 200 characters.")]
        [JsonPropertyName("NonImmigrantExceptionDocs")]
        public string? NonImmigrantExceptionDocs { get; set; }

        /// <summary>
        /// NICS Check Details (Required)
        /// </summary>
        [Required(ErrorMessage = "NICS check details are required.")]
        [JsonPropertyName("NICSCheckDetails")]
        public NICSCheck NICSCheckDetails { get; set; } = default!;

        /// <summary>
        /// Performs validation checks.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Ensure FirearmCategory has unique values
            if (FirearmCategory.Count != FirearmCategory.Distinct().Count())
            {
                yield return new ValidationResult("Each firearm category must be unique.", new[] { nameof(FirearmCategory) });
            }

            // Validate Gun Show Details if provided
            if (GunShowDetails != null)
            {
                if (string.IsNullOrWhiteSpace(GunShowDetails.Name) ||
                    string.IsNullOrWhiteSpace(GunShowDetails.Address) ||
                    string.IsNullOrWhiteSpace(GunShowDetails.City) ||
                    string.IsNullOrWhiteSpace(GunShowDetails.State.ToString())||
                    string.IsNullOrWhiteSpace(GunShowDetails.Zip) ||
                    string.IsNullOrWhiteSpace(GunShowDetails.County))
                {
                    yield return new ValidationResult(
                        "If Gun Show Details are provided, all fields (Name, Address, City, State, Zip, County) must be filled.",
                        new[] { nameof(GunShowDetails) }
                    );
                }
            }
        }
    }
}