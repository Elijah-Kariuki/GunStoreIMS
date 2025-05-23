using System;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for firearm listings, fully compliant with ATF Section A (Item 1-5).
    /// </summary>
    public class FirearmLineDto
    {
        /// <summary>
        /// Unique identifier for the firearm.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Manufacturer or Importer of the firearm (includes PMF designation).
        /// </summary>
        [Required, StringLength(150)]
        public string ManufacturerImporter { get; set; } = default!;

        /// <summary>
        /// Model of the firearm. Optional.
        /// </summary>
        [StringLength(100)]
        public string? Model { get; set; }

        /// <summary>
        /// Serial number of the firearm (uniquely identifying each firearm).
        /// </summary>
        [Required, StringLength(50)]
        public string SerialNumber { get; set; } = default!;

        /// <summary>
        /// Firearm Type (e.g., Pistol, Rifle, Shotgun, Frame, Receiver).
        /// </summary>
        [Required]
        public FirearmEnumType FirearmType { get; set; }

        /// <summary>
        /// Caliber or gauge of the firearm.
        /// </summary>
        [Required, StringLength(50)]
        public string CaliberGauge { get; set; } = default!;

        /// <summary>
        /// Flag indicating if the firearm falls under NFA regulations.
        /// </summary>
        public bool IsNFAItem { get; set; }

        /// <summary>
        /// If NFA, classification (e.g., Machine Gun, Silencer).
        /// </summary>
        public NfaClassification? NfaClass { get; set; }

        /// <summary>
        /// Owning FFL ID (for tracking).
        /// </summary>
        [Required]
        public int FFLId { get; set; }

        /// <summary>
        /// Initial Acquisition Date (tracking when entered into inventory).
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime InitialAcquisitionDate { get; set; }

        /// <summary>
        /// Current status of the firearm (e.g., In Inventory, Transferred, Disposed).
        /// </summary>
        [Required]
        public FirearmStatus CurrentStatus { get; set; }
    }
}
