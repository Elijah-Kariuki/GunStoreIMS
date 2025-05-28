using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionD // Corresponds to $defs/SectionD
    {
        /// <summary>
        /// Buyer Recertification (Only if transfer date differs from Section B)
        /// </summary>
        [JsonPropertyName("BuyerRecertification")]
        public Certification? BuyerRecertification { get; set; }
    }
}