// Domain/Models/Address.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class Address
    {
        [Required, StringLength(100), JsonPropertyName("Street")]
        public string Street { get; set; } = default!;

        [Required, StringLength(60), JsonPropertyName("City")]
        public string City { get; set; } = default!;

        [Required, StringLength(2), JsonPropertyName("State")]
        public string State { get; set; } = default!;

        [Required, StringLength(10), JsonPropertyName("Zip")]
        public string Zip { get; set; } = default!;

        [Required, StringLength(60), JsonPropertyName("County")]
        public string County { get; set; } = default!;

        [JsonPropertyName("ResideInCityLimits")]
        public bool ResideInCityLimits { get; set; }
    }
}
