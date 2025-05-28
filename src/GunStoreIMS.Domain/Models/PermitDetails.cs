using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Domain.Utilities;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class PermitDetails // Corresponds to $defs/PermitDetails
    {
        /// <summary>
        /// Issuing State of the permit
        /// </summary>
        [JsonPropertyName("IssuingState")]
        [EnumDataType(typeof(USState))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public USState? IssuingState { get; set; }

        /// <summary>
        /// Type of the permit
        /// </summary>
        [StringLength(50)]
        [JsonPropertyName("PermitType")]
        public string? PermitType { get; set; }

        /// <summary>
        /// Date the permit was issued
        /// </summary>
        [DataType(DataType.Date)]
        [JsonPropertyName("IssuedDate")]
        [JsonConverter(typeof(DateStringConverter))]
        public DateTime? IssuedDate { get; set; }

        /// <summary>
        /// Expiration date of the permit
        /// </summary>
        [DataType(DataType.Date)]
        [JsonPropertyName("ExpirationDate")]
        [JsonConverter(typeof(DateStringConverter))]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Permit number
        /// </summary>
        [StringLength(50)]
        [JsonPropertyName("PermitNumber")]
        public string? PermitNumber { get; set; }
    }
}