// File: Application/Dto/PlaceOfBirthDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{

    // ======================================================================
    //  PlaceOfBirth DTO (Used within BuyerInfo)
    // ======================================================================
    public class PlaceOfBirthDto : IValidatableObject
    {
        [StringLength(60)]
        [Display(Name = "U.S. City")]
        public string? USCity { get; set; }

        [RegularExpression("^[A-Z]{2}$", ErrorMessage = "State must be a 2-letter code.")]
        [Display(Name = "U.S. State")]
        public string? USState { get; set; }

        [StringLength(60)]
        [Display(Name = "Foreign Country")]
        public string? ForeignCountry { get; set; }

        // Custom validation to enforce the "oneOf" rule from the schema
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool isUsProvided = !string.IsNullOrWhiteSpace(USCity) && !string.IsNullOrWhiteSpace(USState);
            bool isForeignProvided = !string.IsNullOrWhiteSpace(ForeignCountry);

            if (!isUsProvided && !isForeignProvided)
            {
                yield return new ValidationResult("You must provide either U.S. City and State OR a Foreign Country for Place of Birth.",
                    new[] { nameof(USCity), nameof(USState), nameof(ForeignCountry) });
            }
            else if (isUsProvided && isForeignProvided)
            {
                yield return new ValidationResult("You cannot provide both U.S. City/State AND a Foreign Country for Place of Birth.",
                    new[] { nameof(USCity), nameof(USState), nameof(ForeignCountry) });
            }
            else if (!string.IsNullOrWhiteSpace(USCity) && string.IsNullOrWhiteSpace(USState))
            {
                yield return new ValidationResult("U.S. State is required when U.S. City is provided.", new[] { nameof(USState) });
            }
            else if (string.IsNullOrWhiteSpace(USCity) && !string.IsNullOrWhiteSpace(USState))
            {
                yield return new ValidationResult("U.S. City is required when U.S. State is provided.", new[] { nameof(USCity) });
            }
        }
    }

}