using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GunStoreIMS.Domain.Enums;

namespace GunStoreIMS.Domain.Models
{
    public abstract class Firearm : IValidatableObject
    {
        // ─── Identity ──────────────────────────────────────────────────────────
        [Key]
        public Guid Id { get; init; }

        // Manufacturer / Importer / Maker (immutable once created)
        [Required, StringLength(100)]
        public string Manufacturer { get; init; } = default!;

        [StringLength(100)]
        public string? MakerName { get; init; }

        [StringLength(100)]
        public string? Importer { get; init; }

        [StringLength(50)]
        public string? ImporterCity { get; init; }

        public USState? ImporterState { get; init; }

        // Model & serial
        [Required, StringLength(100)]
        public string Model { get; init; } = default!;

        [Required, StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9.\-/]+$", ErrorMessage = "Invalid serial number.")]
        public string SerialNumber { get; init; } = default!;

        [NotMapped]
        public string? DecryptedSerialNumber { get; set; }

        [StringLength(100)]
        public string? AdditionalMarkings { get; set; }

        [Required]
        public bool IsSerialObliterated { get; set; }

        // Caliber (immutable relation)
        [Required]
        public int CaliberId { get; init; }

        [ForeignKey(nameof(CaliberId))]
        public Caliber Caliber { get; init; } = default!;

        // ─── Classification (auto-managed) ────────────────────────────────────
        private static readonly Dictionary<Enums.Type, (bool isNfa, NfaClassification?)> _map = new()
        {
            { Enums.Type.ShortBarreledRifle,   (true, NfaClassification.ShortBarreledRifle) },
            { Enums.Type.ShortBarreledShotgun, (true, NfaClassification.ShortBarreledShotgun) },
            { Enums.Type.MachineGun,           (true, NfaClassification.MachineGun) },
            { Enums.Type.Silencer,             (true, NfaClassification.Silencer) },
            { Enums.Type.DestructiveDevice,    (true, NfaClassification.DestructiveDevice) },
        };

        private Enums.Type _type;

        [Required]
        public Enums.Type Type
        {
            get => _type;
            set
            {
                _type = value;
                if (_map.TryGetValue(value, out var info))
                {
                    IsNFAItem = info.isNfa;
                    NfaClass = info.Item2;
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

        [StringLength(200)]
        public string? OtherTypeDescription { get; set; }

        [NotMapped]
        public bool IsFrameOrReceiver => Type == Enums.Type.Receiver;

        // Manufacture details (immutable)
        [Required, DataType(DataType.Date)]
        public DateTime ManufactureDate { get; init; }

        [Required, StringLength(100)]
        public string CountryOfOrigin { get; init; } = default!;

        public bool IsAntique { get; set; }
        public bool IsImported { get; set; }

        // Owner FFL (immutable once set)
        [Required]
        public int FFLId { get; init; }

        [ForeignKey(nameof(FFLId))]
        public FFL FFL { get; init; } = default!;

        [StringLength(50)]
        public string? YourFFLMarking { get; set; }

        [StringLength(50)]
        public string? YourMarkingLocation { get; set; }
        public bool IsMultiPieceFrame { get; set; }

        // ─── Navigation (read-only) ────────────────────────────────────────────
        private readonly List<Acquisition> _acquisitions = new();
        public IReadOnlyCollection<Acquisition> Acquisitions => _acquisitions.AsReadOnly();

        private readonly List<Disposition> _dispositions = new();
        public IReadOnlyCollection<Disposition> Dispositions => _dispositions.AsReadOnly();

        private readonly List<Recovery> _recoveries = new();
        public IReadOnlyCollection<Recovery> Recoveries => _recoveries.AsReadOnly();

        private readonly List<SerialNumberHistory> _serialHistory = new();
        public IReadOnlyCollection<SerialNumberHistory> SerialHistory => _serialHistory.AsReadOnly();

        // Concurrency / internal
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        [JsonIgnore]
        public string? InternalNotes { get; set; }

        // Domain methods for navigation
        public void AddAcquisition(Acquisition acq)
            => _acquisitions.Add(acq ?? throw new ArgumentNullException(nameof(acq)));

        public bool RemoveAcquisition(Acquisition acq)
            => _acquisitions.Remove(acq);

        public void AddDisposition(Disposition disp)
            => _dispositions.Add(disp ?? throw new ArgumentNullException(nameof(disp)));

        public bool RemoveDisposition(Disposition disp)
            => _dispositions.Remove(disp);

        public void AddRecovery(Recovery rec)
            => _recoveries.Add(rec ?? throw new ArgumentNullException(nameof(rec)));

        public void AddSerialHistory(SerialNumberHistory hist)
            => _serialHistory.Add(hist ?? throw new ArgumentNullException(nameof(hist)));

        // ─── Validation (unchanged except DRY helper) ─────────────────────────
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (ManufactureDate > DateTime.UtcNow)
                yield return Fail(nameof(ManufactureDate), "Manufacture date cannot be in the future.");

            if (IsImported)
            {
                if (string.IsNullOrWhiteSpace(Importer)
                    || string.IsNullOrWhiteSpace(ImporterCity)
                    || !ImporterState.HasValue)
                {
                    yield return Fail(nameof(Importer), "Importer details are required for imported firearms.");
                }

                if (CountryOfOrigin.Equals("USA", StringComparison.OrdinalIgnoreCase))
                {
                    yield return Fail(nameof(CountryOfOrigin), "Imported firearms cannot have 'USA' as Country of Origin.");
                }
            }

            if (SerialNumber?.Length < 3)
                yield return Fail(nameof(SerialNumber), "Serial number must be at least 3 characters long.");

            if (Type == Enums.Type.Other && string.IsNullOrWhiteSpace(OtherTypeDescription))
                yield return Fail(nameof(OtherTypeDescription), "Description is required when firearm type is 'Other'.");
        }

        private static ValidationResult Fail(string member, string msg)
            => new(msg, new[] { member });
    }
}
