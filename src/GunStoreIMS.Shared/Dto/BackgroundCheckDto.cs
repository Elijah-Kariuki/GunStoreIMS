using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Dto
{
    /* ======================================================================
       Background Check DTO – Represents the background check details
       ==================================================================== */
    public class BackgroundCheckDto
    {
        [Required, DataType(DataType.Date), JsonPropertyName("date_contacted")]
        public DateTime DateContacted { get; set; }

        [JsonPropertyName("transaction_number")]
        public string? TransactionNumber { get; set; }

        [Required, JsonPropertyName("initial_response")]
        public string InitialResponse { get; set; } = default!;

        [DataType(DataType.Date), JsonPropertyName("mdi_transfer_date")]
        public DateTime? MDITransferDate { get; set; }

        [JsonPropertyName("subsequent_response")]
        public string? SubsequentResponse { get; set; }

        [DataType(DataType.Date), JsonPropertyName("subsequent_response_date")]
        public DateTime? SubsequentResponseDate { get; set; }

        [JsonPropertyName("no_nics_reason")]
        public string? NoNicsReason { get; set; }

        [JsonPropertyName("permit_details")]
        public PermitDetailsDto? PermitDetails { get; set; }
    }
}