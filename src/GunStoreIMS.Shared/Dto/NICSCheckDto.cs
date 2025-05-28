using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  NICSCheck DTO (Optional, Used within SectionC) - Q27-29
    // ======================================================================
    public class NICSCheckDto : IValidatableObject
    {
        [RegularExpression("^(NFAProcessed|QualifiedPermit)$", ErrorMessage = "Invalid reason.")]
        [Display(Name = "No NICS Required Because (28 or 29)")]
        public string? NoNicsRequired { get; set; } // "NFAProcessed" or "QualifiedPermit"

        [Display(Name = "29. Permit Details")]
        public PermitDetailsDto? PermitDetails { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "27.a. Date to NICS")]
        public DateTime? DateContacted { get; set; }

        [StringLength(50)]
        [Display(Name = "27.b. NICS or State Transaction #")]
        public string? TransactionNumber { get; set; }

        [RegularExpression("^(Proceed|Delayed|Denied|Cancelled)$", ErrorMessage = "Invalid NICS response.")]
        [Display(Name = "27.c. Initial NICS Response")]
        public string? InitialResponse { get; set; } // "Proceed", "Delayed", "Denied", "Cancelled"

        [DataType(DataType.Date)]
        [Display(Name = "27.c. Delayed Transfer (MDI) Date")]
        public DateTime? MDITransferDate { get; set; }

        [RegularExpression("^(Proceed|Denied|Cancelled|Overturned|No response within 3 business days|Additional delay under 21|No response within 10 business days \\(under 21\\))$", ErrorMessage = "Invalid NICS response.")]
        [Display(Name = "27.d. Later NICS Response")]
        public string? LaterResponse { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "27.d. Later NICS Response Date")]
        public DateTime? LaterResponseDate { get; set; }

        [RegularExpression("^(Proceed|Denied|Cancelled)$", ErrorMessage = "Invalid NICS response.")]
        [Display(Name = "27.e. Post-Transfer NICS Response")]
        public string? PostTransferResponse { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "27.e. Post-Transfer NICS Response Date")]
        public DateTime? PostTransferResponseDate { get; set; }

        // Custom validation for NICS/Permit logic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool isNicsExempt = !string.IsNullOrWhiteSpace(NoNicsRequired);
            bool isNicsChecked = DateContacted.HasValue && !string.IsNullOrWhiteSpace(InitialResponse);

            if (!isNicsExempt && !isNicsChecked)
            {
                yield return new ValidationResult("You must either provide NICS check details (Date & Initial Response) or select a 'No NICS Required' reason.",
                    new[] { nameof(DateContacted), nameof(InitialResponse), nameof(NoNicsRequired) });
            }

            if (isNicsExempt && isNicsChecked)
            {
                yield return new ValidationResult("You cannot provide both NICS check details and select a 'No NICS Required' reason.",
                    new[] { nameof(DateContacted), nameof(InitialResponse), nameof(NoNicsRequired) });
            }

            if (NoNicsRequired == "QualifiedPermit" && PermitDetails == null)
            {
                yield return new ValidationResult("Permit Details must be provided when 'QualifiedPermit' is selected.",
                    new[] { nameof(PermitDetails) });
            }

            if (NoNicsRequired != "QualifiedPermit" && PermitDetails != null)
            {
                yield return new ValidationResult("Permit Details should only be provided when 'QualifiedPermit' is selected.",
                     new[] { nameof(PermitDetails) });
            }

            if (InitialResponse == "Delayed" && !MDITransferDate.HasValue)
            {
                // Note: MDI Date is often optional to record, but logically linked.
                // You might adjust this rule based on your specific business process.
                // yield return new ValidationResult("MDI Transfer Date should be provided if the initial response is 'Delayed'.",
                //      new[] { nameof(MDITransferDate) });
            }
        }
    }
}
