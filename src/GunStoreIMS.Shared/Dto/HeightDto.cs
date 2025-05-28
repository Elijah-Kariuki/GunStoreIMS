using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  Height DTO (Used within BuyerInfo)
    // ======================================================================
    public class HeightDto
    {
        [Required]
        [Range(1, 8, ErrorMessage = "Feet must be between 1 and 8.")]
        [Display(Name = "Feet")]
        public int Feet { get; set; }

        [Required]
        [Range(0, 11, ErrorMessage = "Inches must be between 0 and 11.")]
        [Display(Name = "Inches")]
        public int Inches { get; set; }
    }

}
