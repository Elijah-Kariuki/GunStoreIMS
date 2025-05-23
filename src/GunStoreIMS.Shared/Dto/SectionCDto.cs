using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    public class SectionCDto
    {
        [Required]
        public BackgroundCheckDto BackgroundCheck { get; set; } = new();
    }
}
