using System;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;    // for the FirearmEnumType

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for firearm acquisition records, fully optimized for frontend CRUD operations and ATF compliance.
    /// </summary>
    public class AcquisitionRecordDto
    {
        /// <summary>
        /// Unique ID of the acquisition record. Null when creating a new entry.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// ID of the firearm being acquired. Required for linking acquisitions.
        /// </summary>
        [Required(ErrorMessage = "Firearm ID is required.")]
        public Guid FirearmId { get; set; }

        /// <summary>
        /// Type of firearm being acquired (e.g. Pistol, Rifle, etc.).
        /// </summary>
        [Required(ErrorMessage = "Firearm type is required.")]
        public FirearmEnumType FirearmType { get; set; }

        /// <summary>
        /// Serial number of the firearm being acquired. This is critical for ATF compliance.
        /// </summary>
        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(100, ErrorMessage = "Serial number cannot exceed 100 characters.")]
        public string SerialNumber { get; set; } = default!;

        /// <summary>
        /// Date of acquisition.
        /// </summary>
        [Required(ErrorMessage = "Acquisition date is required.")]
        [DataType(DataType.Date)]
        public DateTime AcquisitionDate { get; set; }

        /// <summary>
        /// Name of the source from whom the firearm was acquired.
        /// </summary>
        [Required(ErrorMessage = "Source name is required.")]
        [StringLength(200, ErrorMessage = "Source name cannot exceed 200 characters.")]
        public string SourceName { get; set; } = default!;

        /// <summary>
        /// Address of the source (required if source is a non-licensee).
        /// </summary>
        [StringLength(300, ErrorMessage = "Source address cannot exceed 300 characters.")]
        public string? SourceAddress { get; set; }

        /// <summary>
        /// Federal Firearms License (FFL) number of the source, if applicable.
        /// </summary>
        [StringLength(30, ErrorMessage = "Source license number cannot exceed 30 characters.")]
        [RegularExpression(@"^\d{1,2}-\d{2,3}-\d{3}-\d{2}-\d{2}-\d{5}$", ErrorMessage = "Invalid FFL number format.")]
        public string? SourceLicenseNumber { get; set; }

        /// <summary>
        /// Indicates if the acquisition is part of a private sale.
        /// </summary>
        public bool IsPrivateSale { get; set; }

        /// <summary>
        /// Any relevant acquisition notes (optional but useful for documentation).
        /// </summary>
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; }
    }
}
