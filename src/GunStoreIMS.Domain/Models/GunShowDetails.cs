using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class GunShowDetails // Corresponds to $defs/GunShowDetails
    {
        /// <summary>
        /// 25. Name of Function
        /// </summary>
        [Required(ErrorMessage = "Event Name is required.")]
        [StringLength(100, ErrorMessage = "Event Name cannot exceed 100 characters.")]
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// 25. Address of Event Location
        /// </summary>
        [Required(ErrorMessage = "Event Address is required.")]
        [StringLength(100, ErrorMessage = "Event Address cannot exceed 100 characters.")]
        [JsonPropertyName("Address")]
        public string Address { get; set; } = default!;

        /// <summary>
        /// 25. City of Event Location
        /// </summary>
        [Required(ErrorMessage = "City is required.")]
        [StringLength(60, ErrorMessage = "City cannot exceed 60 characters.")]
        [JsonPropertyName("City")]
        public string City { get; set; } = default!;

        /// <summary>
        /// 25. State of Event Location (Two-letter code via USState enum)
        /// </summary>
        [Required(ErrorMessage = "State is required.")]
        [EnumDataType(typeof(USState))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("State")]
        public USState State { get; set; }

        /// <summary>
        /// 25. ZIP Code of Event Location
        /// </summary>
        [Required(ErrorMessage = "ZIP Code is required.")]
        [StringLength(10, ErrorMessage = "ZIP Code cannot exceed 10 characters.")]
        [RegularExpression("^\\d{5}(?:-\\d{4})?$", ErrorMessage = "ZIP Code must be in 5-digit or ZIP+4 format.")]
        [JsonPropertyName("Zip")]
        public string Zip { get; set; } = default!;

        /// <summary>
        /// 25. County of Event Location
        /// </summary>
        [Required(ErrorMessage = "County is required.")]
        [StringLength(60, ErrorMessage = "County cannot exceed 60 characters.")]
        [JsonPropertyName("County")]
        public string County { get; set; } = default!;
    }
}