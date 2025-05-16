using GunStoreIMS.Domain.Enums;

namespace GunStoreIMS.Domain.Models
{
    public sealed class Rifle : LongGun
    {
        public Rifle() => Type = Type.Rifle;
    }
}
