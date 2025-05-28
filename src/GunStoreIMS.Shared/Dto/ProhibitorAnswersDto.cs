// File: Application/Dto/ProhibitorAnswersDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{

    // ======================================================================
    //  ProhibitorAnswers DTO (Used within SectionB) - Q21
    // ======================================================================
    public class ProhibitorAnswersDto : IValidatableObject
    {
        // NOTE: All questions MUST be answered. `bool` is inherently required,
        // but custom validation will handle the complex inter-dependencies and prohibitions.

        [Display(Name = "21.a. Actual Transferee/Buyer?")]
        public bool IsActualTransferee { get; set; }

        [Display(Name = "21.b. Intent to sell for felony/terrorism/drug trafficking?")]
        public bool IntentToSellFelony { get; set; }

        [Display(Name = "21.c. Under felony indictment?")]
        public bool FelonyIndictment { get; set; }

        [Display(Name = "21.d. Ever convicted of a felony?")]
        public bool FelonyConviction { get; set; }

        [Display(Name = "21.e. Fugitive from justice?")]
        public bool Fugitive { get; set; }

        [Display(Name = "21.f. Unlawful user/addicted to controlled substance?")]
        public bool UnlawfulUser { get; set; }

        [Display(Name = "21.g. Adjudicated mental defective?")]
        public bool AdjudicatedMental { get; set; }

        [Display(Name = "21.h. Dishonorable discharge?")]
        public bool DishonorableDischarge { get; set; }

        [Display(Name = "21.i. Subject to restraining order?")]
        public bool RestrainingOrder { get; set; }

        [Display(Name = "21.j. Domestic violence conviction?")]
        public bool DomesticViolenceConviction { get; set; }

        [Display(Name = "21.k. Renounced U.S. citizenship?")]
        public bool RenouncedCitizenship { get; set; }

        [Display(Name = "21.l. Illegal/unlawful alien?")]
        public bool IllegalAlien { get; set; }

        [Display(Name = "21.m.1. Nonimmigrant visa?")]
        public bool NonImmigrantAlien { get; set; }

        [Display(Name = "21.m.2. Exception applies?")]
        public bool NonImmigrantException { get; set; } // Should only be true if 21.m.1 is true

        [Display(Name = "21.n. Intent to sell to prohibited person?")]
        public bool IntentToSellProhibitedPerson { get; set; }

        // Custom validation for prohibitions and dependencies (implements schema's 'allOf')
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsActualTransferee) yield return new ValidationResult("If 21.a is No, the transaction cannot proceed.", new[] { nameof(IsActualTransferee) });
            if (IntentToSellFelony) yield return new ValidationResult("If 21.b is Yes, the transaction cannot proceed.", new[] { nameof(IntentToSellFelony) });
            if (FelonyIndictment) yield return new ValidationResult("If 21.c is Yes, the transaction cannot proceed.", new[] { nameof(FelonyIndictment) });
            if (FelonyConviction) yield return new ValidationResult("If 21.d is Yes, the transaction cannot proceed.", new[] { nameof(FelonyConviction) });
            if (Fugitive) yield return new ValidationResult("If 21.e is Yes, the transaction cannot proceed.", new[] { nameof(Fugitive) });
            if (UnlawfulUser) yield return new ValidationResult("If 21.f is Yes, the transaction cannot proceed.", new[] { nameof(UnlawfulUser) });
            if (AdjudicatedMental) yield return new ValidationResult("If 21.g is Yes, the transaction cannot proceed.", new[] { nameof(AdjudicatedMental) });
            if (DishonorableDischarge) yield return new ValidationResult("If 21.h is Yes, the transaction cannot proceed.", new[] { nameof(DishonorableDischarge) });
            if (RestrainingOrder) yield return new ValidationResult("If 21.i is Yes, the transaction cannot proceed.", new[] { nameof(RestrainingOrder) });
            if (DomesticViolenceConviction) yield return new ValidationResult("If 21.j is Yes, the transaction cannot proceed.", new[] { nameof(DomesticViolenceConviction) });
            if (RenouncedCitizenship) yield return new ValidationResult("If 21.k is Yes, the transaction cannot proceed.", new[] { nameof(RenouncedCitizenship) });
            if (IllegalAlien) yield return new ValidationResult("If 21.l is Yes, the transaction cannot proceed.", new[] { nameof(IllegalAlien) });
            if (IntentToSellProhibitedPerson) yield return new ValidationResult("If 21.n is Yes, the transaction cannot proceed.", new[] { nameof(IntentToSellProhibitedPerson) });

            if (NonImmigrantAlien && !NonImmigrantException)
            {
                yield return new ValidationResult("If 21.m.1 is Yes, then 21.m.2 must also be Yes and documentation provided.", new[] { nameof(NonImmigrantException) });
            }
            if (!NonImmigrantAlien && NonImmigrantException)
            {
                yield return new ValidationResult("21.m.2 should only be Yes if 21.m.1 is Yes.", new[] { nameof(NonImmigrantException) });
            }
        }
    }
}
