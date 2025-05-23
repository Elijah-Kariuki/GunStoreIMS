// Domain/Enums/SexOption.cs
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SexOption
{
    [EnumMember(Value = "Male")]
    Male,

    [EnumMember(Value = "Female")]
    Female,

    [EnumMember(Value = "Non‑Binary")]
    NonBinary
}
