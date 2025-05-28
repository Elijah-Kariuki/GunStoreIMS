using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionB
    {
        /// <summary>
        /// Buyer Information (Required)
        /// </summary>
        [Required(ErrorMessage = "BuyerInfo is required.")]
        [JsonPropertyName("BuyerInfo")]
        public BuyerInfo BuyerInfo { get; set; } = default!;

        /// <summary>
        /// Prohibitor Answers (Question 21) (Required)
        /// </summary>
        [Required(ErrorMessage = "ProhibitorAnswers are required.")]
        [JsonPropertyName("ProhibitorAnswers")]
        public Question21 ProhibitorAnswers { get; set; } = default!;

        /// <summary>
        /// Buyer Certification (Required)
        /// </summary>
        [Required(ErrorMessage = "BuyerCertification is required.")]
        [JsonPropertyName("BuyerCertification")]
        public Certification BuyerCertification { get; set; } = default!;
    }
}