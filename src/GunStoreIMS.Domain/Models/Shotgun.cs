using GunStoreIMS.Domain.Enums;

namespace GunStoreIMS.Domain.Models
{
    public sealed class Shotgun : LongGun
    {
        public Shotgun() => Type = Type.Shotgun;
    }
}
