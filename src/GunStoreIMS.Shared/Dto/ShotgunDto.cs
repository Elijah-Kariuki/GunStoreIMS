using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for shotgun details, including barrel and overall length.
    /// Inherits all standard firearm detail fields from LongGunDto.
    /// </summary>
    public class ShotgunDto : LongGunDto
    {
        public ShotgunDto()
        {
            // Set the enum‐backed Type property (inherited from FirearmDetailDto)
            Type = FirearmType.Shotgun; // Use 'this' to explicitly reference the property
        }
    }
}
