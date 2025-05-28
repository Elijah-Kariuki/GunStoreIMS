// File: Domain/Models/BuyerRecertification.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore; // For [Owned]

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents the buyer's recertification data, aligning with $defs/Certification in the JSON schema.
    /// This is used in Section D.
    /// </summary>
    [Owned]
    public class BuyerRecertification // This class structure matches $defs/Certification
    {
        /// <summary>
        /// The signature image for recertification.
        /// </summary>
        [Required(ErrorMessage = "Recertification Signature is required.")]
        [JsonPropertyName("Signature")] // Matches $defs/Certification
        public string Signature { get; set; } = default!;

        /// <summary>
        /// The date of recertification.
        /// </summary>
        [Required(ErrorMessage = "Recertification Date is required.")]
        [DataType(DataType.Date)]
        [JsonPropertyName("DateSigned")] // Matches $defs/Certification
        public DateTime DateSigned { get; set; }
    }
}