using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  Certification DTO (Used within SectionB and SectionD)
    // ======================================================================
    public class CertificationDto
    {
        [Required(ErrorMessage = "Signature is required.")]
        [Display(Name = "Signature")]
        public string Signature { get; set; } = default!; // Base64 Encoded String

        [Required(ErrorMessage = "Certification Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Certification Date")]
        public DateTime DateSigned { get; set; }
    }
}