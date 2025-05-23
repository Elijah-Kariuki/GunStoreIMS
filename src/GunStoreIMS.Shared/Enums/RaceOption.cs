// Domain/Enums/RaceOption.cs
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RaceOption
{
    [EnumMember(Value = "American Indian or Alaska Native")]
    AmericanIndianOrAlaskaNative,

    [EnumMember(Value = "Asian")]
    Asian,

    [EnumMember(Value = "Black or African American")]
    BlackOrAfricanAmerican,

    [EnumMember(Value = "Native Hawaiian or Other Pacific Islander")]
    NativeHawaiianOrOtherPacificIslander,

    [EnumMember(Value = "White")]
    White
}
