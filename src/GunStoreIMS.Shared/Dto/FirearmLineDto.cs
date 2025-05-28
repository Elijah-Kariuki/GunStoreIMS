using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums; // Assuming FirearmType enum is here

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for firearm listings, fully compliant with ATF Section A (Item 1–5).
    /// </summary>
    public class FirearmLineDto
    {
        /// <summary>
        /// Gets or sets the Manufacturer and Importer (Question 1).
        /// </summary>
        [JsonPropertyName("ManufacturerImporter")]
        [Required(ErrorMessage = "Manufacturer/Importer is required.")]
        [StringLength(150, ErrorMessage = "Manufacturer/Importer cannot exceed 150 characters.")]
        public string ManufacturerImporter { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Model (Question 2).
        /// </summary>
        [JsonPropertyName("Model")]
        [Required(ErrorMessage = "Model is required.")]
        [StringLength(60, ErrorMessage = "Model cannot exceed 60 characters.")]
        public string Model { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Serial Number (Question 3). Use 'NSN' if none.
        /// </summary>
        [JsonPropertyName("SerialNumber")]
        [Required(ErrorMessage = "Serial Number is required.")]
        [StringLength(60, ErrorMessage = "Serial Number cannot exceed 60 characters.")]
        public string SerialNumber { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Type of firearm (Question 4).
        /// This should correspond to the ATF Form 4473 defined types.
        /// </summary>
        [JsonPropertyName("Type")]
        [Required(ErrorMessage = "Type is required.")]
        public FirearmType Type { get; set; } // Enum from GunStoreIMS.Shared.Enums

        /// <summary>
        /// Gets or sets the Caliber or Gauge (Question 5).
        /// </summary>
        [JsonPropertyName("CaliberGauge")]
        [Required(ErrorMessage = "Caliber or Gauge is required.")]
        [StringLength(30, ErrorMessage = "Caliber or Gauge cannot exceed 30 characters.")]
        public string CaliberGauge { get; set; } = default!;
    }
}