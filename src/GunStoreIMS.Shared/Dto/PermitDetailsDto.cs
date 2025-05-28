// File: Application/Dto/PermitDetailsDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{

    // ======================================================================
    //  PermitDetails DTO (Optional, Used within NICSCheckDto) - Q29
    // ======================================================================
    public class PermitDetailsDto
    {
        [RegularExpression("^[A-Z]{2}$", ErrorMessage = "State must be a 2-letter code.")]
        [Display(Name = "29. Issuing State")]
        public string? IssuingState { get; set; }

        [StringLength(50)]
        [Display(Name = "29. Permit Type")]
        public string? PermitType { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "29. Issued Date")]
        public DateTime? IssuedDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "29. Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [StringLength(50)]
        [Display(Name = "29. Permit Number")]
        public string? PermitNumber { get; set; }
    }
}