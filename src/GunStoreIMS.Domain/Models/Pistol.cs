using GunStoreIMS.Domain.Enums;

namespace GunStoreIMS.Domain.Models
{
    public sealed class Pistol : Handgun
    {
        public Pistol() => Type = Type.Pistol;
    }

}
