using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class Question21 : IValidatableObject // Corresponds to $defs/Question21
    {
        /// <summary>
        /// 21.a. Actual Transferee?
        /// Must be Yes.
        /// </summary>
        [Required(ErrorMessage = "Must confirm actual transferee.")]
        [JsonPropertyName("IsActualTransferee")]
        public bool IsActualTransferee { get; set; }

        /// <summary>
        /// 21.b. Intent to sell for felony/terrorism/drug trafficking?
        /// Must be No.
        /// </summary>
        [Required(ErrorMessage = "Intent to sell for felony/terrorism/drug trafficking must be answered.")]
        [JsonPropertyName("IntentToSellFelony")]
        public bool IntentToSellFelony { get; set; }

        // Repeated structure for other eligibility questions
        [Required][JsonPropertyName("FelonyIndictment")] public bool FelonyIndictment { get; set; }
        [Required][JsonPropertyName("FelonyConviction")] public bool FelonyConviction { get; set; }
        [Required][JsonPropertyName("Fugitive")] public bool Fugitive { get; set; }
        [Required][JsonPropertyName("UnlawfulUser")] public bool UnlawfulUser { get; set; }
        [Required][JsonPropertyName("AdjudicatedMental")] public bool AdjudicatedMental { get; set; }
        [Required][JsonPropertyName("DishonorableDischarge")] public bool DishonorableDischarge { get; set; }
        [Required][JsonPropertyName("RestrainingOrder")] public bool RestrainingOrder { get; set; }
        [Required][JsonPropertyName("DomesticViolenceConviction")] public bool DomesticViolenceConviction { get; set; }
        [Required][JsonPropertyName("RenouncedCitizenship")] public bool RenouncedCitizenship { get; set; }
        [Required][JsonPropertyName("IllegalAlien")] public bool IllegalAlien { get; set; }
        [Required][JsonPropertyName("NonImmigrantAlien")] public bool NonImmigrantAlien { get; set; }
        [JsonPropertyName("NonImmigrantException")] public bool? NonImmigrantException { get; set; } 
        [Required][JsonPropertyName("IntentToSellProhibitedPerson")] public bool IntentToSellProhibitedPerson { get; set; }

        /// <summary>
        /// Validation rules based on conditional requirements from the JSON schema.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsActualTransferee)
            {
                yield return new ValidationResult("If 21.a is No, transaction cannot proceed.", new[] { nameof(IsActualTransferee) });
            }
            if (IntentToSellFelony)
            {
                yield return new ValidationResult("If 21.b is Yes, transaction cannot proceed.", new[] { nameof(IntentToSellFelony) });
            }
            if (FelonyIndictment)
            {
                yield return new ValidationResult("If 21.c is Yes, transaction cannot proceed.", new[] { nameof(FelonyIndictment) });
            }
            if (FelonyConviction)
            {
                yield return new ValidationResult("If 21.d is Yes, transaction cannot proceed.", new[] { nameof(FelonyConviction) });
            }
            if (Fugitive)
            {
                yield return new ValidationResult("If 21.e is Yes, transaction cannot proceed.", new[] { nameof(Fugitive) });
            }
            if (UnlawfulUser)
            {
                yield return new ValidationResult("If 21.f is Yes, transaction cannot proceed.", new[] { nameof(UnlawfulUser) });
            }
            if (AdjudicatedMental)
            {
                yield return new ValidationResult("If 21.g is Yes, transaction cannot proceed.", new[] { nameof(AdjudicatedMental) });
            }
            if (DishonorableDischarge)
            {
                yield return new ValidationResult("If 21.h is Yes, transaction cannot proceed.", new[] { nameof(DishonorableDischarge) });
            }
            if (RestrainingOrder)
            {
                yield return new ValidationResult("If 21.i is Yes, transaction cannot proceed.", new[] { nameof(RestrainingOrder) });
            }
            if (DomesticViolenceConviction)
            {
                yield return new ValidationResult("If 21.j is Yes, transaction cannot proceed.", new[] { nameof(DomesticViolenceConviction) });
            }
            if (RenouncedCitizenship)
            {
                yield return new ValidationResult("If 21.k is Yes, transaction cannot proceed.", new[] { nameof(RenouncedCitizenship) });
            }
            if (IllegalAlien)
            {
                yield return new ValidationResult("If 21.l is Yes, transaction cannot proceed.", new[] { nameof(IllegalAlien) });
            }
            if (IntentToSellProhibitedPerson)
            {
                yield return new ValidationResult("If 21.n is Yes, transaction cannot proceed.", new[] { nameof(IntentToSellProhibitedPerson) });
            }
            if (NonImmigrantAlien && (NonImmigrantException == null || !NonImmigrantException.Value))
            {
                yield return new ValidationResult("If 21.m.1 is Yes, then 21.m.2 must be Yes and documentation provided.", new[] { nameof(NonImmigrantAlien), nameof(NonImmigrantException) });
            }
        }
    }
}