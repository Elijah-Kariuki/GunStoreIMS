// Domain/Models/Form4473Record.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a complete ATF Form 4473 transaction record.
    /// Designed for EF Core persistence and JSON serialization for REST APIs.
    /// </summary>
    public class Form4473Record : IValidatableObject
    {
        [Key]
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [StringLength(50)]
        [JsonPropertyName("TransactionNumber")]
        public string? TransactionNumber { get; set; }

        [JsonPropertyName("ContinuationSheet")]
        public bool ContinuationSheet { get; set; }

        [Required]
        [JsonPropertyName("SectionA")]
        public SectionA SectionA { get; set; } = new();

        [Required]
        [JsonPropertyName("SectionB")]
        public SectionB SectionB { get; set; } = new();

        [Required]
        [JsonPropertyName("SectionC")]
        public SectionC SectionC { get; set; } = new();

        [JsonPropertyName("SectionD")]
        public SectionD? SectionD { get; set; }

        [Required]
        [JsonPropertyName("SectionE")]
        public SectionE SectionE { get; set; } = new();

        // --- Relationships ---
        public Guid BuyerInfoId { get; set; }
        public BuyerInfo BuyerInfo { get; set; } = null!;

        public Guid DealerRecordId { get; set; }
        public DealerRecord DealerRecord { get; set; } = null!;

        public DealerInfo SectionEDealerInfo { get; set; } = new();

        // --- Firearm lines come in via SectionA, but persisted via this collection ---
        [JsonIgnore]
        public ICollection<Form4473FirearmLine> Form4473FirearmLines { get; set; } = new List<Form4473FirearmLine>();

        // --- Other direct properties ---
        [StringLength(100)]
        public string? PawnRedemptionLines { get; set; }

        [StringLength(50)]
        public string? TotalNumber { get; set; }

        [StringLength(200)]
        public string? SupplementalDocs { get; set; }

        [StringLength(200)]
        public string? DealerNotes { get; set; }

        [StringLength(50)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Status { get; set; }

        [DataType(DataType.DateTime)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime LastModifiedAt { get; set; }

        [Timestamp]
        [JsonIgnore]
        public byte[]? RowVersion { get; set; }

        // --- Owned types (no separate Ids) ---
        public Certification Certification { get; set; } = new();
        public BuyerRecertification BuyerRecertification { get; set; } = new();
        public ProhibitorAnswers ProhibitorAnswers { get; set; } = new();
        public NICSCheck NICSChecks { get; set; } = new();
        public NonImmigrantExceptionDocs NonImmigrantExceptionDocs { get; set; } = new();
        public TransferorCertification TransferorCertification { get; set; } = new();
        public GunShowDetails GunShowDetails { get; set; } = new();
        public Identification Identification { get; set; } = new();
        public PCSOrders PCSOrders { get; set; } = new();

        /// <summary>
        /// Prepares SectionA.Firearms for JSON serialization from EF Core collection.
        /// Call before returning to API clients.
        /// </summary>
        public void MapFirearmLinesToSectionAForSerialization()
        {
            SectionA.Firearms = Form4473FirearmLines
                .Select(fl => new Form4473FirearmLine
                {
                    ManufacturerImporter = fl.ManufacturerImporter,
                    Model = fl.Model,
                    SerialNumber = fl.SerialNumber,
                    Type = fl.Type,
                    CaliberGauge = fl.CaliberGauge
                })
                .ToList();
        }

        /// <summary>
        /// Populates the EF Core Form4473FirearmLines collection from deserialized SectionA.Firearms.
        /// Call after model binding and before SaveChanges.
        /// </summary>
        public void MapSectionAFirearmsToDomainCollection()
        {
            Form4473FirearmLines = SectionA.Firearms
                .Select(fa => new Form4473FirearmLine
                {
                    Form4473Record = this,
                    Form4473RecordId = this.Id,
                    ManufacturerImporter = fa.ManufacturerImporter,
                    Model = fa.Model,
                    SerialNumber = fa.SerialNumber,
                    Type = fa.Type,
                    CaliberGauge = fa.CaliberGauge
                })
                .ToList();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // ContinuationSheet logic
            int count = SectionA.Firearms?.Count ?? Form4473FirearmLines.Count;
            if (count >= 4 && !ContinuationSheet)
                yield return new ValidationResult(
                    "When 4+ firearms are listed, ContinuationSheet must be true.",
                    new[] { nameof(ContinuationSheet) });

            if (count < 4 && ContinuationSheet)
                yield return new ValidationResult(
                    "ContinuationSheet should only be true if 4 or more firearms are listed.",
                    new[] { nameof(ContinuationSheet) });

            // Validation for Nonimmigrant Exception Documents
            if (SectionB.ProhibitorAnswers.NonImmigrantAlien
                 && SectionB.ProhibitorAnswers.NonImmigrantException
                 && string.IsNullOrWhiteSpace(SectionC.NonImmigrantExceptionDocs))
            {
                yield return new ValidationResult(
                    "If Q21.m.1 & Q21.m.2 are Yes, then Section C nonimmigrant exception docs must be provided.",
                    new[] { "SectionC.NonImmigrantExceptionDocs" });
            }


            // Recertification date vs initial certification date
            if (SectionD?.BuyerRecertification != null)
            {
                var d0 = SectionB.BuyerCertification.DateSigned.Date;
                var d1 = SectionD.BuyerRecertification.DateSigned.Date;
                if (d1 <= d0)
                {
                    yield return new ValidationResult(
                        "Section D recertification date must be after Section B certification date.",
                        new[] { "SectionD.BuyerRecertification.DateSigned" });
                }
            }

            // Age vs category rules
            var birth = SectionB.BuyerInfo.BirthDate;
            int age = DateTime.Today.Year - birth.Year - (birth > DateTime.Today.AddYears(-(DateTime.Today.Year - birth.Year)) ? 1 : 0);
            bool isHandgun = SectionC.FirearmCategory.Any(c =>
                c.Contains("Handgun", StringComparison.OrdinalIgnoreCase)
                || c.Contains("Other Firearm", StringComparison.OrdinalIgnoreCase)
                || c.Contains("Frame", StringComparison.OrdinalIgnoreCase)
                || c.Contains("Receiver", StringComparison.OrdinalIgnoreCase));

            if (isHandgun && age < 21)
                yield return new ValidationResult(
                    "Buyer must be at least 21 for handguns or other frames/receivers.",
                    new[] { "SectionB.BuyerInfo.BirthDate", "SectionC.FirearmCategory" });

            bool isLongGun = SectionC.FirearmCategory.Any(c =>
                c.Contains("Long Gun", StringComparison.OrdinalIgnoreCase)
                || c.Contains("Rifle", StringComparison.OrdinalIgnoreCase)
                || c.Contains("Shotgun", StringComparison.OrdinalIgnoreCase));

            if (isLongGun && age < 18)
                yield return new ValidationResult(
                    "Buyer must be at least 18 for long guns.",
                    new[] { "SectionB.BuyerInfo.BirthDate", "SectionC.FirearmCategory" });
        }
    }
}
