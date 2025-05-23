using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// DTO for pistol details, extending the base handgun fields.
    /// </summary>
    public class PistolDto : HandgunDto
    {
        public PistolDto()
        {
            // Set the enum‐backed Type property inherited from FirearmDetailDto
            Type = FirearmType.Pistol;
        }
    }
}
