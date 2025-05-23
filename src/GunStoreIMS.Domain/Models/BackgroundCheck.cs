// Domain/Models/BackgroundCheck.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class BackgroundCheck
    {
        [Required, DataType(DataType.Date), JsonPropertyName("DateContacted")]
        public DateTime DateContacted { get; set; }

        [JsonPropertyName("TransactionNumber")]
        public string? TransactionNumber { get; set; }

        [Required, JsonPropertyName("InitialResponse")]
        public string InitialResponse { get; set; } = default!;

        [DataType(DataType.Date), JsonPropertyName("MDITransferDate")]
        public DateTime? MDITransferDate { get; set; }

        [JsonPropertyName("SubsequentResponse")]
        public string? SubsequentResponse { get; set; }

        [DataType(DataType.Date), JsonPropertyName("SubsequentResponseDate")]
        public DateTime? SubsequentResponseDate { get; set; }

        [JsonPropertyName("NoNicsReason")]
        public string? NoNicsReason { get; set; }

        [JsonPropertyName("PermitDetails")]
        public PermitDetails? PermitDetails { get; set; }
    }

   
}
