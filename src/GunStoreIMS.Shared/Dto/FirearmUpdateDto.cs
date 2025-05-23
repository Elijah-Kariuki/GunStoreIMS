using System;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for updating an existing firearm while maintaining ATF compliance.
    /// </summary>
    public class FirearmUpdateDto
    {
        /// <summary>
        /// Unique identifier of the firearm being updated.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Optional description if firearm type is "Other."
        /// </summary>
        [StringLength(200)]
        public string? OtherTypeDescription { get; set; }

        /// <summary>
        /// Barrel length (mandatory for NFA firearms).
        /// </summary>
        [Range(0.1, 100.0)]
        public decimal? BarrelLength { get; set; }

        /// <summary>
        /// Overall length (mandatory for NFA firearms).
        /// </summary>
        [Range(0.1, 200.0)]
        public decimal? OverallLength { get; set; }

        /// <summary>
        /// Current status of the firearm (e.g., In Inventory, Transferred, Disposed).
        /// </summary>
        [Required]
        public FirearmStatus CurrentStatus { get; set; }

        /// <summary>
        /// Licensee’s identifying marking (if required).
        /// </summary>
        [StringLength(150)]
        public string? YourFFLMarking { get; set; }

        /// <summary>
        /// Location of licensee's marking.
        /// </summary>
        [StringLength(100)]
        public string? YourMarkingLocation { get; set; }

        /// <summary>
        /// Internal notes for firearm tracking and compliance documentation.
        /// </summary>
        [StringLength(1000)]
        public string? InternalNotes { get; set; }

        /// <summary>
        /// Date of disposition (if applicable).
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? DateOfDisposition { get; set; }

        /// <summary>
        /// Classification of the firearm under NFA, if applicable.
        /// </summary>
        public NfaClassification? NfaClass { get; set; }

        /// <summary>
        /// Indicates whether the firearm is an NFA item.
        /// </summary>
        public bool IsNFAItem { get; set; }
    }
}
