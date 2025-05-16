using GunStoreIMS.Domain.Enums;

namespace GunStoreIMS.Domain.Models
{
    public sealed class Revolver : Handgun
    {
        public Revolver() => Type = Type.Revolver;
    }
}
