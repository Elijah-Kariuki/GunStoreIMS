using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums; // Required if you map LicenseType

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for Dealer Records, aligned with the DealerRecord domain model.
    /// </summary>
    public class DealerRecordDto
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonPropertyName("recordDate")]
        public DateTime RecordDate { get; set; }

        [Required]
        [StringLength(120)]
        [JsonPropertyName("tradeName")]
        public string TradeName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [JsonPropertyName("fflNumber")]
        public string FFLNumber { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("licenseType")]
        public string LicenseType { get; set; } = string.Empty; // Consider using FflLicenseType enum

        [Required]
        [DataType(DataType.Date)]
        [JsonPropertyName("licenseExpirationDate")]
        public DateTime LicenseExpirationDate { get; set; } // Matches old DTO, map from ExpirationDateUtc

        [Required]
        [StringLength(150)]
        [JsonPropertyName("addressLine1")]
        public string AddressLine1 { get; set; } = string.Empty; // Replaces StreetAddress

        [StringLength(150)]
        [JsonPropertyName("addressLine2")]
        public string? AddressLine2 { get; set; } // Added

        [Required]
        [StringLength(100)]
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty; // Updated length, kept ? for older compatibility but model requires it

        [Required]
        [RegularExpression("^[A-Z]{2}$", ErrorMessage = "Use a valid two-letter state code.")]
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty; // Updated to required, kept ? for older compatibility but model requires it

        [Required]
        [RegularExpression(@"^\d{5}(?:-\d{4})?$", ErrorMessage = "Use 12345 or 12345-6789 format.")]
        [JsonPropertyName("zipCode")] // Changed from zip
        public string ZipCode { get; set; } = string.Empty; // Updated to required, kept ? for older compatibility but model requires it

        [Phone]
        [StringLength(25)]
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; } // Added

        [EmailAddress]
        [StringLength(100)]
        [JsonPropertyName("email")]
        public string? Email { get; set; } // Added

        // NOTE: RecordDate and IsAcquisition were removed as they are not in DealerRecord model.
        // If needed, they belong elsewhere or the model needs changing.
    }
}