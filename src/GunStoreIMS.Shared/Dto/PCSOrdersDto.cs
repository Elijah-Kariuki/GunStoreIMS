using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  PCSOrders DTO (Optional, Used within SectionC) - Q26.c
    // ======================================================================
    public class PCSOrdersDto
    {
        [StringLength(150)]
        [Display(Name = "26.c. PCS Base, City and State")]
        public string? PCSBaseCityState { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "26.c. PCS Effective Date")]
        public DateTime? PCSEffectiveDate { get; set; }

        [StringLength(50)]
        [Display(Name = "26.c. PCS Order Number")]
        public string? PCSOrderNumber { get; set; }
    }
}
