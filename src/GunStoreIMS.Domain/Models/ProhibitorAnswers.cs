// Domain/Models/ProhibitorAnswers.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class ProhibitorAnswers : IValidatableObject
    {
        [Required, JsonPropertyName("IsActualTransferee")]
        public bool IsActualTransferee { get; set; }

        [Required, JsonPropertyName("FelonyIndictment")]
        public bool FelonyIndictment { get; set; }

        [Required, JsonPropertyName("FelonyConviction")]
        public bool FelonyConviction { get; set; }

        [Required, JsonPropertyName("Fugitive")]
        public bool Fugitive { get; set; }

        [Required, JsonPropertyName("UnlawfulUser")]
        public bool UnlawfulUser { get; set; }

        [Required, JsonPropertyName("AdjudicatedMental")]
        public bool AdjudicatedMental { get; set; }

        [Required, JsonPropertyName("DishonorableDischarge")]
        public bool DishonorableDischarge { get; set; }

        [Required, JsonPropertyName("RestrainingOrder")]
        public bool RestrainingOrder { get; set; }

        [Required, JsonPropertyName("DomesticViolenceConviction")]
        public bool DomesticViolenceConviction { get; set; }

        [Required, JsonPropertyName("RenouncedCitizenship")]
        public bool RenouncedCitizenship { get; set; }

        [Required, JsonPropertyName("IllegalAlien")]
        public bool IllegalAlien { get; set; }

        [Required, JsonPropertyName("NonImmigrantAlien")]
        public bool NonImmigrantAlien { get; set; }

        [Required, JsonPropertyName("NonImmigrantException")]
        public bool NonImmigrantException { get; set; }

        [Required, JsonPropertyName("IntentToSellFelony")]
        public bool IntentToSellFelony { get; set; }

        [JsonPropertyName("IntentToSellProhibitedPerson")]
        public bool? IntentToSellProhibitedPerson { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (NonImmigrantAlien && !NonImmigrantException)
            {
                yield return new ValidationResult(
                    "If NonImmigrantAlien is true, NonImmigrantException must also be true.",
                    new[] { nameof(NonImmigrantException) });
            }
        }
    }
}
