using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  Section C DTO – Identification & NICS (Main Container)
    // ======================================================================
    public class SectionCDto
    {
        /// <summary>
        /// Gets or sets the categories of firearms being transferred (Question 24).
        /// </summary>
        [Required(ErrorMessage = "At least one Firearm Category must be selected.")]
        [MinLength(1, ErrorMessage = "At least one Firearm Category must be selected.")]
        [Display(Name = "24. Category of firearm(s)")]
        public List<string> FirearmCategory { get; set; } = new(); // "Handgun", "Long Gun", "Other Firearm"

        /// <summary>
        /// Gets or sets details if the transfer occurs at a gun show (Question 25). Optional.
        /// </summary>
        [Display(Name = "25. Gun Show/Event Details")]
        public GunShowDetailsDto? GunShowDetails { get; set; }

        /// <summary>
        /// Gets or sets the buyer's identification details (Question 26.a).
        /// </summary>
        [Required(ErrorMessage = "Identification Details are required.")]
        [Display(Name = "26.a. Identification")]
        public IdentificationDto Identification { get; set; } = new();

        /// <summary>
        /// Gets or sets supplemental documentation details (Question 26.b). Optional.
        /// </summary>
        [StringLength(200)]
        [Display(Name = "26.b. Supplemental Documentation")]
        public string? SupplementalDocs { get; set; }

        /// <summary>
        /// Gets or sets Permanent Change of Station (PCS) order details (Question 26.c). Optional.
        /// </summary>
        [Display(Name = "26.c. PCS Orders")]
        public PCSOrdersDto? PCSOrders { get; set; }

        /// <summary>
        /// Gets or sets nonimmigrant alien exception documentation details (Question 26.d). Optional.
        /// </summary>
        [StringLength(200)]
        [Display(Name = "26.d. Nonimmigrant Exception Docs")]
        public string? NonImmigrantExceptionDocs { get; set; }

        /// <summary>
        /// Gets or sets the NICS background check details (Questions 27-29).
        /// NOTE: While the object itself isn't 'Required', its internal logic (via IValidatableObject)
        /// ensures that *either* a NICS check *or* an exemption is recorded.
        /// </summary>
        [Display(Name = "27-29. NICS/State POC Check")]
        public NICSCheckDto NICSCheckDetails { get; set; } = new();
    }
}
