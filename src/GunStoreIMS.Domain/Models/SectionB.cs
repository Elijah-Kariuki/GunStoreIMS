// Domain/Models/SectionB.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionB
    {
        [Required, JsonPropertyName("BuyerInfo")]
        public BuyerInfo BuyerInfo { get; set; } = new();

        [Required, JsonPropertyName("ProhibitorAnswers")]
        public ProhibitorAnswers ProhibitorAnswers { get; set; } = new();

        [Required, JsonPropertyName("BuyerCertification")]
        public BuyerCertification BuyerCertification { get; set; } = new();
    }
}
