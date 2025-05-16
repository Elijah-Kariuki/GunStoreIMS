// Domain/Models/DealerInfo.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class DealerInfo
    {
        [Required, StringLength(100), JsonPropertyName("TradeName")]
        public string TradeName { get; set; } = default!;

        [Required, StringLength(100), JsonPropertyName("StreetAddress")]
        public string StreetAddress { get; set; } = default!;

        [StringLength(60), JsonPropertyName("City")]
        public string? City { get; set; }

        [StringLength(2), JsonPropertyName("State")]
        public string? State { get; set; }

        [StringLength(10), JsonPropertyName("Zip")]
        public string? Zip { get; set; }

        [Required, RegularExpression(@"^[0-9]-[0-9]{2}-[0-9]{5}$"), JsonPropertyName("FFLNumber")]
        public string FFLNumber { get; set; } = default!;
    }
}
