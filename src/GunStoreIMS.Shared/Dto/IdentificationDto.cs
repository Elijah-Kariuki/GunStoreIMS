using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  Identification DTO (Used within SectionC) - Q26.a
    // ======================================================================
    public class IdentificationDto
    {
        [Required(ErrorMessage = "Issuing Authority and Type are required.")]
        [StringLength(100)]
        [Display(Name = "26.a. Issuing Authority and Type")]
        public string IssuingAuthorityAndType { get; set; } = default!;

        [Required(ErrorMessage = "Identification Number is required.")]
        [StringLength(50)]
        [Display(Name = "26.a. Number")]
        public string Number { get; set; } = default!;

        [Required(ErrorMessage = "Expiration Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "26.a. Expiration Date")]
        public DateTime ExpirationDate { get; set; }
    }
}
