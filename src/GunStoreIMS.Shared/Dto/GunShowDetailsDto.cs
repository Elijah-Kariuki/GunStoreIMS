using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  GunShowDetails DTO (Optional, Used within SectionC) - Q25
    // ======================================================================
    public class GunShowDetailsDto
    {
        [StringLength(100)]
        [Display(Name = "25. Name of Function")]
        public string? Name { get; set; }

        [StringLength(100)]
        [Display(Name = "25. Address")]
        public string? Address { get; set; }

        [StringLength(60)]
        [Display(Name = "25. City")]
        public string? City { get; set; }

        [RegularExpression("^[A-Z]{2}$", ErrorMessage = "State must be a 2-letter code.")]
        [Display(Name = "25. State")]
        public string? State { get; set; }

        [RegularExpression("^\\d{5}(?:-\\d{4})?$", ErrorMessage = "ZIP Code must be 5 or 9 digits.")]
        [Display(Name = "25. ZIP Code")]
        public string? Zip { get; set; }

        [StringLength(60)]
        [Display(Name = "25. County")]
        public string? County { get; set; }
    }
}
