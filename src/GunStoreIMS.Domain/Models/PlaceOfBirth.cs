using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class PlaceOfBirth :IValidatableObject// Corresponds to $defs/PlaceOfBirth
    {
        /// <summary>
        /// 11. US City of Birth (Required if USState is provided)
        /// </summary>
        [StringLength(60, ErrorMessage = "US City cannot exceed 60 characters.")]
        [JsonPropertyName("USCity")]
        public string? USCity { get; set; }

        /// <summary>
        /// 11. US State of Birth (Required if USCity is provided)
        /// </summary>
        [EnumDataType(typeof(USState))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("USState")]
        public USState? USState { get; set; }

        /// <summary>
        /// 11. Foreign Country of Birth (Required if USCity and USState are not provided)
        /// </summary>
        [StringLength(60, ErrorMessage = "Foreign Country cannot exceed 60 characters.")]
        [JsonPropertyName("ForeignCountry")]
        public string? ForeignCountry { get; set; }

        /// <summary>
        /// Ensures only USCity+USState OR ForeignCountry is provided, not both.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool isUSBirth = !string.IsNullOrWhiteSpace(USCity) && USState.HasValue;
            bool isForeignBirth = !string.IsNullOrWhiteSpace(ForeignCountry);

            if (!isUSBirth && !isForeignBirth)
            {
                yield return new ValidationResult("Provide either USCity+USState or ForeignCountry.", new[] { nameof(USCity), nameof(USState), nameof(ForeignCountry) });
            }

            if (isUSBirth && isForeignBirth)
            {
                yield return new ValidationResult("Cannot provide both USCity+USState and ForeignCountry.", new[] { nameof(USCity), nameof(USState), nameof(ForeignCountry) });
            }
        }
    }
}