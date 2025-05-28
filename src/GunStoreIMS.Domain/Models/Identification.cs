using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class Identification // Corresponds to $defs/Identification
    {
        /// <summary>
        /// 26.a. Issuing Authority and Type (e.g., VA DL)
        /// </summary>
        [Required(ErrorMessage = "Issuing Authority and Type are required.")]
        [StringLength(100, ErrorMessage = "Issuing Authority and Type cannot exceed 100 characters.")]
        [JsonPropertyName("IssuingAuthorityAndType")]
        public string IssuingAuthorityAndType { get; set; } = default!;

        /// <summary>
        /// 26.a. Identification Number
        /// </summary>
        [Required(ErrorMessage = "Identification Number is required.")]
        [StringLength(50, ErrorMessage = "Identification Number cannot exceed 50 characters.")]
        [JsonPropertyName("Number")]
        public string Number { get; set; } = default!;

        /// <summary>
        /// 26.a. Expiration Date
        /// </summary>
        [Required(ErrorMessage = "Expiration Date is required.")]
        [DataType(DataType.Date)]
        [JsonPropertyName("ExpirationDate")]
        [JsonConverter(typeof(DateStringConverter))] // Ensuring alignment with JSON schema reference
        public DateTime ExpirationDate { get; set; }
    }
}