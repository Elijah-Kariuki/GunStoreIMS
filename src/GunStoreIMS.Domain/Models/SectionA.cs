// Domain/Models/SectionA.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionA : IValidatableObject
    {
        [Required, MinLength(1), MaxLength(29)]
        [JsonPropertyName("Firearms")]
        public List<Form4473FirearmLine> Firearms { get; set; } = new();

        [Required, StringLength(50)]
        [RegularExpression(
            "^(one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|[A-Za-z ]+)$",
            ErrorMessage = "Must be spelled-out (e.g. “one”, “two”).")]
        [JsonPropertyName("TotalNumber")]
        public string TotalNumber { get; set; } = default!;

        [JsonPropertyName("IsPawnRedemption")]
        public bool IsPawnRedemption { get; set; }

        [JsonPropertyName("IsPrivatePartyTransfer")]
        public bool IsPrivatePartyTransfer { get; set; }

        [JsonPropertyName("ContinuationSheet")]
        public bool? ContinuationSheet { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (Firearms.Count >= 4 && ContinuationSheet != true)
                yield return new ValidationResult(
                    "When 4+ firearms are listed, ContinuationSheet must be checked.",
                    new[] { nameof(ContinuationSheet) });
        }
    }
}
