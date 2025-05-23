using GunStoreIMS.Shared.Enums;  // for the FirearmType enum
namespace GunStoreIMS.Shared.Dto
{
    public class RevolverDto : HandgunDto
    {
        public RevolverDto()
        {
            // Ensure the "Type" enum is set correctly
            Type = FirearmType.Revolver;
        }
    }
}
