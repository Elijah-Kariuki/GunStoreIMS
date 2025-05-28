using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class DealerInfo // Corresponds to $defs/DealerInfo
    {
        /// <summary>
        /// 33. Dealer Trade Name
        /// </summary>
        [Required(ErrorMessage = "Trade Name is required.")]
        [StringLength(100, ErrorMessage = "Trade Name cannot exceed 100 characters.")]
        [JsonPropertyName("TradeName")]
        public string TradeName { get; set; } = default!;

        /// <summary>
        /// Dealer Street Address (Number and Street)
        /// </summary>
        [Required(ErrorMessage = "Street Address is required.")]
        [StringLength(100, ErrorMessage = "Street Address cannot exceed 100 characters.")]
        [JsonPropertyName("StreetAddress")]
        public string StreetAddress { get; set; } = default!;

        /// <summary>
        /// Dealer City
        /// </summary>
        [Required(ErrorMessage = "City is required.")]
        [StringLength(60, ErrorMessage = "City cannot exceed 60 characters.")]
        [JsonPropertyName("City")]
        public string City { get; set; } = default!;

        /// <summary>
        /// Dealer State (Two-letter code via USState enum)
        /// </summary>
        [Required(ErrorMessage = "State is required.")]
        [EnumDataType(typeof(USState))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("State")]
        public USState State { get; set; }

        /// <summary>
        /// Dealer ZIP Code (5-digit or ZIP+4)
        /// </summary>
        [Required(ErrorMessage = "ZIP Code is required.")]
        [StringLength(10, ErrorMessage = "ZIP Code cannot exceed 10 characters.")]
        [RegularExpression("^\\d{5}(?:-\\d{4})?$", ErrorMessage = "ZIP Code must be in 5-digit or ZIP+4 format.")]
        [JsonPropertyName("Zip")]
        public string Zip { get; set; } = default!;

        /// <summary>
        /// Dealer FFL Number (15-char format X-XX-XXX-XX-XX-XXXXX)
        /// </summary>
        [Required(ErrorMessage = "FFL Number is required.")]
        [RegularExpression("^[0-9]-[0-9]{2}-[0-9]{3}-[0-9]{2}-[0-9]{2}-[0-9]{5}$", ErrorMessage = "FFL Number must match format X-XX-XXX-XX-XX-XXXXX.")]
        [JsonPropertyName("FFLNumber")]
        public string FFLNumber { get; set; } = default!;
    }
}