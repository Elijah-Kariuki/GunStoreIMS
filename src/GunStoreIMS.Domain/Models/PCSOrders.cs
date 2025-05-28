using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class PCSOrders // Corresponds to $defs/PCSOrders
    {
        /// <summary>
        /// 26.c. PCS Base City and State
        /// </summary>
        [Required(ErrorMessage = "PCS Base City and State is required.")]
        [StringLength(150, ErrorMessage = "PCS Base City and State cannot exceed 150 characters.")]
        [JsonPropertyName("PCSBaseCityState")]
        public string PCSBaseCityState { get; set; } = default!;

        /// <summary>
        /// 26.c. PCS Effective Date
        /// </summary>
        [Required(ErrorMessage = "PCS Effective Date is required.")]
        [DataType(DataType.Date)]
        [JsonPropertyName("PCSEffectiveDate")]
        [JsonConverter(typeof(DateStringConverter))] // Ensuring alignment with JSON schema reference
        public DateTime PCSEffectiveDate { get; set; }

        /// <summary>
        /// 26.c. PCS Order Number
        /// </summary>
        [Required(ErrorMessage = "PCS Order Number is required.")]
        [StringLength(50, ErrorMessage = "PCS Order Number cannot exceed 50 characters.")]
        [JsonPropertyName("PCSOrderNumber")]
        public string PCSOrderNumber { get; set; } = default!;
    }
}