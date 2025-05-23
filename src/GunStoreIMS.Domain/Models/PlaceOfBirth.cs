// Domain/Models/PlaceOfBirth.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class PlaceOfBirth
    {
        [JsonPropertyName("USCity")]
        public string? USCity { get; set; }

        [StringLength(2), JsonPropertyName("USState")]
        public string? USState { get; set; }

        [Required, JsonPropertyName("Country")]
        public string Country { get; set; } = default!;
    }
}
