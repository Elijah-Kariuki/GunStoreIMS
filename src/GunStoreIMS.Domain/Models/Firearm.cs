using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums; // This brings NfaClassification, FirearmStatus, USState directly into scope


namespace GunStoreIMS.Domain.Models
{
    public abstract class Firearm : IValidatableObject
    {
        // --- Identity ---
        [Key]
        public Guid Id { get; set; }

        // --- Manufacturer / Importer / Maker (Immutable once correctly recorded) ---
        [Required(ErrorMessage = "Manufacturer is required.")]
        [StringLength(100, ErrorMessage = "Manufacturer name cannot exceed 100 characters.")]
        public string Manufacturer { get; init; } = default!;

        [StringLength(100, ErrorMessage = "Maker name cannot exceed 100 characters.")]
        public string? MakerName { get; init; }

        [StringLength(100, ErrorMessage = "Importer name cannot exceed 100 characters.")]
        public string? ImporterName { get; init; }

        [StringLength(50, ErrorMessage = "Importer city cannot exceed 50 characters.")]
        public string? ImporterCity { get; init; }

        public USState? ImporterState { get; init; }

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(100, ErrorMessage = "Model name cannot exceed 100 characters.")]
        public string Model { get; init; } = default!;

        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(50, ErrorMessage = "Serial number cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-z0-9.\-/]+$", ErrorMessage = "Serial number contains invalid characters. Allowed: A-Z, a-z, 0-9, '.', '-', '/'")]
        public string SerialNumber { get; init; } = default!;

        [NotMapped]
        public string? DecryptedSerialNumber { get; set; }

        [StringLength(255, ErrorMessage = "Additional markings cannot exceed 255 characters.")]
        public string? AdditionalMarkings { get; set; }

        [Required]
        public bool IsSerialObliterated { get; set; }

        // --- Caliber / Gauge (Immutable relation) ---
        [Required(ErrorMessage = "Caliber ID is required.")]
        public int CaliberId { get; init; }

        [ForeignKey(nameof(CaliberId))]
        public virtual Caliber Caliber { get; init; } = default!;

        // --- Physical Characteristics ---
        [Range(0.1, 100.0, ErrorMessage = "Barrel length must be between 0.1 and 100 inches.")]
        public decimal? BarrelLength { get; set; }

        [Range(0.1, 200.0, ErrorMessage = "Overall length must be between 0.1 and 200 inches.")]
        public decimal? OverallLength { get; set; }

        // --- Classification (auto-managed based on FirearmType) ---
        private static readonly Dictionary<FirearmEnumType, (bool isNfa, NfaClassification? nfaClass)> _nfaTypeMap = new()
        {
            { FirearmEnumType.ShortBarreledRifle, (true, NfaClassification.ShortBarreledRifle) },
            { FirearmEnumType.ShortBarreledShotgun, (true, NfaClassification.ShortBarreledShotgun) },
            { FirearmEnumType.Machinegun, (true, NfaClassification.MachineGun) },
            { FirearmEnumType.Silencer, (true, NfaClassification.Silencer) },
            { FirearmEnumType.DestructiveDevice, (true, NfaClassification.DestructiveDevice) },
            { FirearmEnumType.AnyOtherWeapon, (true, NfaClassification.AnyOtherWeapon) }
            // Add other relevant types if they exist in your FirearmEnumType enum
        };

        private FirearmEnumType _type;

        [Required(ErrorMessage = "Firearm type is required.")]
        public FirearmEnumType FirearmType // Property type is now the alias
        {
            get => _type;
            init
            {
                _type = value; // value is of type FirearmEnumType
                if (_nfaTypeMap.TryGetValue(value, out var info))
                {
                    IsNFAItem = info.isNfa;
                    NfaClass = info.nfaClass;
                }
                else
                {
                    IsNFAItem = false;
                    NfaClass = null;
                }
            }
        }

        public bool IsNFAItem { get; private set; }
        public NfaClassification? NfaClass { get; private set; }

        [StringLength(200, ErrorMessage = "Other type description cannot exceed 200 characters.")]
        public string? OtherTypeDescription { get; set; }

        [NotMapped]
        public bool IsFrameOrReceiver => FirearmType == FirearmEnumType.Receiver || FirearmType == FirearmEnumType.Frame; // Use alias

        // --- Manufacture Details (Immutable) ---
        [Required(ErrorMessage = "Manufacture date is required.")]
        [DataType(DataType.Date)]
        public DateTime ManufactureDate { get; init; }

        [Required(ErrorMessage = "Country of Origin is required.")]
        [StringLength(100, ErrorMessage = "Country of Origin cannot exceed 100 characters.")]
        public string CountryOfOrigin { get; init; } = default!;

        public bool IsAntique { get; init; }
        public bool IsImported { get; init; }

        // --- FFL Specific Information & PMF Markings ---
        /// <summary>
        /// Foreign key linking to the DealerRecord that owns this firearm.
        /// </summary>
        [Required(ErrorMessage = "Owning Dealer Record ID is required.")]
        public Guid DealerRecordId { get; init; } // <--- CHANGED: Now Guid, renamed to DealerRecordId

        /// <summary>
        /// Navigation property to the owning DealerRecord.
        /// </summary>
        [ForeignKey(nameof(DealerRecordId))] // <--- CHANGED: Points to DealerRecordId
        public virtual DealerRecord DealerRecord { get; init; } = default!; // <--- CHANGED: Now DealerRecord type and name

        [StringLength(150, ErrorMessage = "Your FFL Marking cannot exceed 150 characters.")]
        public string? YourFFLMarking { get; set; }

        [StringLength(100, ErrorMessage = "Your Marking Location cannot exceed 100 characters.")]
        public string? YourMarkingLocation { get; set; }

        public bool IsPrivatelyMadeFirearm { get; set; }
        public bool IsMultiPieceFrame { get; init; }

        // --- A&D Record Information ---
        [Required(ErrorMessage = "Initial acquisition date is required.")]
        [DataType(DataType.Date)]
        public DateTime InitialAcquisitionDate { get; set; }

        [Required]
        public FirearmStatus CurrentStatus { get; set; } = FirearmStatus.InInventory;

        [DataType(DataType.Date)]
        public DateTime? DateOfDisposition { get; set; }

        // --- Navigation (read-only collections for history) ---
        public virtual ICollection<AcquisitionRecord> AcquisitionRecords { get; private set; } = new List<AcquisitionRecord>();
        public virtual ICollection<DispositionRecord> DispositionRecords { get; private set; } = new List<DispositionRecord>();
        public virtual ICollection<SerialNumberHistory> SerialNumberHistories { get; private set; } = new List<SerialNumberHistory>();

        // --- Concurrency / Internal Use ---
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        [JsonIgnore]
        [StringLength(1000, ErrorMessage = "Internal notes cannot exceed 1000 characters.")]
        public string? InternalNotes { get; set; }

        // --- Domain Methods for Managing Collections ---
        public void AddAcquisitionRecord(AcquisitionRecord acq)
        {
            if (acq == null) throw new ArgumentNullException(nameof(acq));
            (AcquisitionRecords as List<AcquisitionRecord>)?.Add(acq);
            if (AcquisitionRecords.Count == 1)
            {
                this.InitialAcquisitionDate = acq.AcquisitionDate;
            }
        }

        public void AddDispositionRecord(DispositionRecord disp)
        {
            if (disp == null) throw new ArgumentNullException(nameof(disp));
            (DispositionRecords as List<DispositionRecord>)?.Add(disp);
            this.CurrentStatus = FirearmStatus.Disposed;
            this.DateOfDisposition = disp.TransactionDate;
        }

        // --- Validation Logic (ATF Compliance Checks) ---
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ManufactureDate > DateTime.UtcNow)
                yield return new ValidationResult("Manufacture date cannot be in the future.", new[] { nameof(ManufactureDate) });

            if (InitialAcquisitionDate > DateTime.UtcNow)
                yield return new ValidationResult("Acquisition date cannot be in the future.", new[] { nameof(InitialAcquisitionDate) });

            if (DateOfDisposition.HasValue && DateOfDisposition.Value > DateTime.UtcNow)
                yield return new ValidationResult("Disposition date cannot be in the future.", new[] { nameof(DateOfDisposition) });

            if (DateOfDisposition.HasValue && DateOfDisposition.Value < InitialAcquisitionDate)
                yield return new ValidationResult("Disposition date cannot be before the acquisition date.", new[] { nameof(DateOfDisposition), nameof(InitialAcquisitionDate) });

            if (IsImported)
            {
                if (string.IsNullOrWhiteSpace(ImporterName) || string.IsNullOrWhiteSpace(ImporterCity) || !ImporterState.HasValue)
                    yield return new ValidationResult("Full Importer details (Name, City, State) are required for imported firearms.", new[] { nameof(ImporterName), nameof(ImporterCity), nameof(ImporterState) });

                if (CountryOfOrigin.Equals("USA", StringComparison.OrdinalIgnoreCase) || CountryOfOrigin.Equals("United States", StringComparison.OrdinalIgnoreCase))
                    yield return new ValidationResult("Imported firearms cannot have 'USA' or 'United States' as Country of Origin.", new[] { nameof(CountryOfOrigin) });
            }
            else
            {
                if (!CountryOfOrigin.Equals("USA", StringComparison.OrdinalIgnoreCase) && !CountryOfOrigin.Equals("United States", StringComparison.OrdinalIgnoreCase))
                    yield return new ValidationResult("Non-imported firearms should typically have 'USA' or 'United States' as Country of Origin unless specific circumstances apply.", new[] { nameof(CountryOfOrigin) });
            }

            if (SerialNumber != null && SerialNumber.Length < 1)
                yield return new ValidationResult("Serial number must be at least 1 character long.", new[] { nameof(SerialNumber) });

            if ((this.FirearmType == FirearmEnumType.Other || this.FirearmType == FirearmEnumType.Frame || this.FirearmType == FirearmEnumType.Receiver) && string.IsNullOrWhiteSpace(OtherTypeDescription)) // Use alias
                yield return new ValidationResult("A description is required when firearm type is 'Other', 'Frame', or 'Receiver'.", new[] { nameof(OtherTypeDescription) });

            if (IsNFAItem)
            {
                if (!BarrelLength.HasValue || BarrelLength <= 0)
                    yield return new ValidationResult("Barrel length is required for NFA firearms.", new[] { nameof(BarrelLength) });
                if (!OverallLength.HasValue || OverallLength <= 0)
                    yield return new ValidationResult("Overall length is required for NFA firearms.", new[] { nameof(OverallLength) });

                if (this.FirearmType == FirearmEnumType.ShortBarreledRifle && BarrelLength >= 16) // Use alias
                    yield return new ValidationResult("A Short-Barreled Rifle typically has a barrel length less than 16 inches.", new[] { nameof(BarrelLength) });
            }

            if (IsPrivatelyMadeFirearm && CurrentStatus == FirearmStatus.InInventory)
            {
                if (string.IsNullOrWhiteSpace(YourFFLMarking))
                    yield return new ValidationResult("FFL marking (Licensee Name/Abbreviation, City, State) is required for Privately Made Firearms acquired or made by the FFL.", new[] { nameof(YourFFLMarking) });
            }
        }
    }
}