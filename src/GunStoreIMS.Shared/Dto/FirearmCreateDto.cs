using System;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for creating a new firearm entry in compliance with ATF regulations.
    /// </summary>
    public class FirearmCreateDto
    {
        /// <summary>
        /// Manufacturer or Importer of the firearm.
        /// </summary>
        [Required, StringLength(100)]
        public string Manufacturer { get; init; } = default!;

        /// <summary>
        /// Maker's Name (for PMF or custom builds).
        /// </summary>
        [StringLength(100)]
        public string? MakerName { get; init; }

        /// <summary>
        /// Model of the firearm.
        /// </summary>
        [Required, StringLength(100)]
        public string Model { get; init; } = default!;

        /// <summary>
        /// Serial number of the firearm.
        /// </summary>
        [Required, StringLength(50)]
        public string SerialNumber { get; init; } = default!;

        /// <summary>
        /// Type of firearm (e.g., Pistol, Rifle, Shotgun).
        /// </summary>
        [Required]
        public FirearmEnumType FirearmType { get; init; }

        /// <summary>
        /// Additional description if firearm type is "Other".
        /// </summary>
        [StringLength(200)]
        public string? OtherTypeDescription { get; init; }

        /// <summary>
        /// Linked caliber ID (foreign key reference).
        /// </summary>
        [Required]
        public int CaliberId { get; init; }

        /// <summary>
        /// Barrel length (required for certain classifications).
        /// </summary>
        [Range(0.1, 100.0)]
        public decimal? BarrelLength { get; init; }

        /// <summary>
        /// Overall length (required for NFA classifications).
        /// </summary>
        [Range(0.1, 200.0)]
        public decimal? OverallLength { get; init; }

        /// <summary>
        /// Country of origin.
        /// </summary>
        [Required, StringLength(100)]
        public string CountryOfOrigin { get; init; } = default!;

        /// <summary>
        /// Date of manufacture.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime ManufactureDate { get; init; }

        /// <summary>
        /// Indicates if the firearm was imported.
        /// </summary>
        public bool IsImported { get; init; }

        /// <summary>
        /// Importer name (if applicable).
        /// </summary>
        [StringLength(100)]
        public string? ImporterName { get; init; }

        /// <summary>
        /// Importer city.
        /// </summary>
        [StringLength(50)]
        public string? ImporterCity { get; init; }

        /// <summary>
        /// Importer state.
        /// </summary>
        public USState? ImporterState { get; init; }

        /// <summary>
        /// Flags firearm as Privately Made Firearm (PMF).
        /// </summary>
        public bool IsPrivatelyMadeFirearm { get; init; }

        /// <summary>
        /// Indicates if the firearm is a multi-piece frame.
        /// </summary>
        public bool IsMultiPieceFrame { get; init; }

        /// <summary>
        /// Indicates if the firearm is classified as an antique.
        /// </summary>
        public bool IsAntique { get; init; }

        /// <summary>
        /// Flags the serial number as obliterated.
        /// </summary>
        public bool IsSerialObliterated { get; init; }

        /// <summary>
        /// Additional firearm markings.
        /// </summary>
        [StringLength(255)]
        public string? AdditionalMarkings { get; init; }

        /// <summary>
        /// Owning FFL ID (required).
        /// </summary>
        [Required]
        public int FFLId { get; init; }
    }
}
