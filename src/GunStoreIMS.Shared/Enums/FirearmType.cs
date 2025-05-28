// Shared/Enums/FirearmType.cs
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FirearmType
    {
        [EnumMember(Value = "Pistol")]
        Pistol,
       
        [EnumMember(Value = "Revolver")]
        Revolver,
        
        [EnumMember(Value = "Rifle")]
        Rifle,
        
        [EnumMember(Value = "Shotgun")]
        Shotgun,
        
        [EnumMember(Value = "Receiver")]
        Receiver,
        
        [EnumMember(Value = "Frame")]
        Frame,
        
        [EnumMember(Value = "Other")]
        Other,
        
        [EnumMember(Value = "Silencer")]
        Silencer,
        
        [EnumMember(Value = "Machinegun")]
        Machinegun,
        
        [EnumMember(Value = "Short-barreled shotgun")]
        ShortBarreledShotgun,
        
        [EnumMember(Value = "Short-barreled rifle")]
        ShortBarreledRifle,
        
        [EnumMember(Value = "Destructive device")]
        DestructiveDevice,
        
        [EnumMember(Value = "Any other weapon")]
        AnyOtherWeapon
    }
}
