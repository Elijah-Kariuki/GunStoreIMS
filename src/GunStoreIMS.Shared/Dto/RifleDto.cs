using GunStoreIMS.Shared.Enums;  // for the FirearmType enum
namespace GunStoreIMS.Shared.Dto
{
    public class RifleDto : LongGunDto
    {
        public RifleDto()
        {
            // Ensure the "Type" enum is set correctly
            Type = FirearmType.Rifle;
        }
    }
}
