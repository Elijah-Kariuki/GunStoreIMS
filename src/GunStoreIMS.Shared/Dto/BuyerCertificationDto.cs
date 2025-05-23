using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Shared.Dto
{
    /* -- B.3 Buyer signature & date -------------------------------------- */
    public class BuyerCertificationDto
    {
        /// <summary>Base-64 encoded PNG/JPEG captured by front-end signature pad.</summary>
        [Required, StringLength(500_000)] // Limits size to ~500 KB (adjust as needed)
        [JsonPropertyName("signature_image")]
        public string SignatureImage { get; set; } = default!;

        [Required, DataType(DataType.Date)]
        [JsonPropertyName("date_signed")]
        public DateTime DateSigned { get; set; }
    }
}