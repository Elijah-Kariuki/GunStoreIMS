// Domain/Models/PermitDetails.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class PermitDetails
    {
        [StringLength(2), JsonPropertyName("IssuingState")]
        public string? IssuingState { get; set; }

        [JsonPropertyName("PermitType")]
        public string? PermitType { get; set; }

        [DataType(DataType.Date), JsonPropertyName("IssuedDate")]
        public DateTime? IssuedDate { get; set; }

        [DataType(DataType.Date), JsonPropertyName("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonPropertyName("PermitNumber")]
        public string? PermitNumber { get; set; }
    }
}