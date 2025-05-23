using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{
    public class AddressDto
    {
        [Required, StringLength(100), JsonPropertyName("street")]
        public string Street { get; set; } = default!;

        [Required, StringLength(60), JsonPropertyName("city")]
        public string City { get; set; } = default!;

        [StringLength(60), JsonPropertyName("county")]
        public string? County { get; set; }

        [Required, JsonPropertyName("state")]
        public USState State { get; set; }

        [Required, StringLength(10), JsonPropertyName("zip")]
        public string Zip { get; set; } = default!;

        [JsonPropertyName("reside_in_city_limits")]
        public bool? ResideInCityLimits { get; set; }
    }
}