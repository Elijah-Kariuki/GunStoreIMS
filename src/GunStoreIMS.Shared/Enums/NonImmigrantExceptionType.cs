using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Enums
{
    public enum NonImmigrantExceptionType
    {
        [Display(Name = "Hunting License/Permit")]
        HuntingLicense,
        [Display(Name = "Waiver from Attorney General")]
        Waiver,
        [Display(Name = "Official Representative")]
        OfficialRep,
        [Display(Name = "Law Enforcement")]
        LawEnforcement,
        [Display(Name = "Other")]
        Other
    }
}
