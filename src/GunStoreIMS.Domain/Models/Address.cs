using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class Address // Corresponds to $defs/Address
    {
        /// <summary>
        /// Number and Street (Cannot be a P.O. Box)
        /// </summary>
        [Required(ErrorMessage = "Street address is required.")]
        [StringLength(100, ErrorMessage = "Street address cannot exceed 100 characters.")]
        [RegularExpression("^(?!.*[Pp]\\.? *[Oo]\\.? *[Bb][Oo][Xx])", ErrorMessage = "Street Address cannot be a P.O. Box.")]
        [JsonPropertyName("Street")]
        public string Street { get; set; } = default!;

        /// <summary>
        /// City
        /// </summary>
        [Required(ErrorMessage = "City is required.")]
        [StringLength(60, ErrorMessage = "City cannot exceed 60 characters.")]
        [JsonPropertyName("City")]
        public string City { get; set; } = default!;

        /// <summary>
        /// State (Two-letter code via USState enum)
        /// </summary>
        [Required(ErrorMessage = "State is required.")]
        [EnumDataType(typeof(USState))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("State")]
        public USState State { get; set; }

        /// <summary>
        /// ZIP Code
        /// </summary>
        [Required(ErrorMessage = "ZIP Code is required.")]
        [StringLength(10, ErrorMessage = "ZIP Code cannot exceed 10 characters.")]
        [RegularExpression("^\\d{5}(?:-\\d{4})?$", ErrorMessage = "ZIP Code must be in 5-digit or ZIP+4 format.")]
        [JsonPropertyName("Zip")]
        public string Zip { get; set; } = default!;

        /// <summary>
        /// County/Parish/Borough
        /// </summary>
        [Required(ErrorMessage = "County is required.")]
        [StringLength(60, ErrorMessage = "County cannot exceed 60 characters.")]
        [JsonPropertyName("County")]
        public string County { get; set; } = default!;

        /// <summary>
        /// Reside in City Limits?
        /// </summary>
        [Required(ErrorMessage = "Must specify if residing within city limits.")]
        [JsonPropertyName("ResideInCityLimits")]
        [EnumDataType(typeof(YesNoUnknown))] // Corrected the enum reference
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public YesNoUnknown ResideInCityLimits { get; set; }
    }
}