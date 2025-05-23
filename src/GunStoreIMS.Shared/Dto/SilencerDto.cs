using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;  // for the FirearmType enum

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for silencer (NFA item) details.
    /// Inherits all the standard detail fields, plus overall length.
    /// </summary>
    public class SilencerDto : FirearmDetailDto
    {
        public SilencerDto()
        {
            // Ensure the "Type" enum is set correctly
            Type = FirearmType.Silencer;
        }

        [Range(1, 40, ErrorMessage = "Overall length must be between 1 and 40 inches.")]
        public decimal OverallLengthInches { get; set; }
    }
}
