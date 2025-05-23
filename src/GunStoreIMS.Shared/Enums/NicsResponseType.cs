// Domain/Enums/NicsResponseType.cs
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NicsResponseType
{
    Proceed,
    Delayed,
    Denied,
    Cancelled,
    Overturned
}
