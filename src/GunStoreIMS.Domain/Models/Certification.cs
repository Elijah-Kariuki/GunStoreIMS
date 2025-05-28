using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class Certification // Corresponds to $defs/Certification
    {
        /// <summary>
        /// Signature (Base64 JPEG/PNG)
        /// </summary>
        [Required(ErrorMessage = "Signature is required.")]
        [JsonPropertyName("Signature")]
        public string Signature { get; set; } = default!;

        /// <summary>
        /// Date Signed
        /// </summary>
        [Required(ErrorMessage = "Date Signed is required.")]
        [DataType(DataType.Date)]
        [JsonPropertyName("DateSigned")]
        [JsonConverter(typeof(DateStringConverter))] // Ensuring alignment with JSON schema reference
        public DateTime DateSigned { get; set; }
    }
}