using System;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums; // Assuming you have or will have enums like FirearmType here

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for detailed firearm information, used for display and CRUD operations on individual firearms.
    /// Optimized for ATF compliance and comprehensive record-keeping.
    /// </summary>
    public class FirearmDetailDto
    {
        /// <summary>
        /// Unique ID of the firearm record. Null when creating a new firearm entry.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Serial number of the firearm. This is a critical identifier.
        /// </summary>
        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(100, ErrorMessage = "Serial number cannot exceed 100 characters.")]
        public string SerialNumber { get; set; } = default!;

        /// <summary>
        /// Manufacturer of the firearm.
        /// </summary>
        [Required(ErrorMessage = "Manufacturer is required.")]
        [StringLength(150, ErrorMessage = "Manufacturer name cannot exceed 150 characters.")]
        public string Manufacturer { get; set; } = default!;

        /// <summary>
        /// Model designation of the firearm.
        /// </summary>
        [Required(ErrorMessage = "Model is required.")]
        [StringLength(100, ErrorMessage = "Model name cannot exceed 100 characters.")]
        public string Model { get; set; } = default!;

        /// <summary>
        /// Type of the firearm (e.g., Pistol, Rifle, Shotgun).
        /// This should align with ATF classifications.
        /// </summary>
        [Required(ErrorMessage = "Firearm type is required.")]
        public FirearmType Type { get; set; } // Assuming FirearmType is an enum in GunStoreIMS.Shared.Enums

        /// <summary>
        /// Caliber or gauge of the firearm.
        /// </summary>
        [Required(ErrorMessage = "Caliber or gauge is required.")]
        [StringLength(50, ErrorMessage = "Caliber/gauge cannot exceed 50 characters.")]
        public string CaliberOrGauge { get; set; } = default!;

        /// <summary>
        /// Name of the importer, if the firearm was imported.
        /// </summary>
        [StringLength(150, ErrorMessage = "Importer name cannot exceed 150 characters.")]
        public string? ImporterName { get; set; }

        /// <summary>
        /// Address of the importer, if the firearm was imported.
        /// </summary>
        [StringLength(300, ErrorMessage = "Importer address cannot exceed 300 characters.")]
        public string? ImporterAddress { get; set; }

        /// <summary>
        /// Country where the firearm was manufactured.
        /// </summary>
        [StringLength(100, ErrorMessage = "Country of manufacture cannot exceed 100 characters.")]
        public string? CountryOfManufacture { get; set; }

        /// <summary>
        /// General description of the firearm, including features like finish, barrel length, sights, etc.
        /// </summary>
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        /// <summary>
        /// Indicates if the firearm is new or used.
        /// </summary>
        public bool IsNew { get; set; } = true; // Default to true, can be set to false for used firearms

        /// <summary>
        /// Any additional notes specific to this firearm (e.g., condition details, accessories included).
        /// </summary>
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; }

        /// <summary>
        /// Barrel length of the firearm (in inches).
        /// </summary>
        [Range(1, 50, ErrorMessage = "Barrel length must be between 1 and 50 inches.")]
        public decimal? BarrelLength { get; set; }

        /// <summary>
        /// Finish of the firearm (e.g., Matte Black, Stainless Steel, FDE).
        /// </summary>
        [StringLength(100, ErrorMessage = "Finish description cannot exceed 100 characters.")]
        public string? Finish { get; set; }

        /// <summary>
        /// Date the firearm was manufactured.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? DateOfManufacture { get; set; }
    }
}
