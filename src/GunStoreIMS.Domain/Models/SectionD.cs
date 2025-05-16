// Domain/Models/SectionD.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionD
    {
        [JsonPropertyName("BuyerRecertification")]
        public BuyerRecertification? BuyerRecertification { get; set; }
    }

    [Owned]
    public class BuyerRecertification
    {
        [Required, JsonPropertyName("Signature")]
        public string SignatureImage { get; set; } = default!;

        [Required, JsonPropertyName("DateRecertified")]
        public DateTime DateRecertified { get; set; }
    }
}
