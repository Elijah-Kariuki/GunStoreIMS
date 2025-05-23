// Domain/Enums/EthnicityOption.cs
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EthnicityOption
{
    [EnumMember(Value = "Hispanic or Latino")]
    HispanicOrLatino,

    [EnumMember(Value = "Not Hispanic or Latino")]
    NotHispanicOrLatino
}
