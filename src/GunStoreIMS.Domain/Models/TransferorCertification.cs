using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using GunStoreIMS.Domain.Utilities;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class TransferorCertification // Corresponds to $defs/TransferorCertification
    {
        /// <summary>
        /// 34. Transferor's Name
        /// </summary>
        [Required(ErrorMessage = "Transferor Name is required.")]
        [StringLength(100, ErrorMessage = "Transferor Name cannot exceed 100 characters.")]
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// 35. Transferor's Title
        /// </summary>
        [Required(ErrorMessage = "Transferor Title is required.")]
        [StringLength(100, ErrorMessage = "Transferor Title cannot exceed 100 characters.")]
        [JsonPropertyName("Title")]
        public string Title { get; set; } = default!;

        /// <summary>
        /// Transferor Signature (Base64 JPEG/PNG)
        /// </summary>
        [Required(ErrorMessage = "Transferor Signature is required.")]
        [JsonPropertyName("SignatureImage")]
        public string SignatureImage { get; set; } = default!;

        /// <summary>
        /// 36. Transfer Date
        /// </summary>
        [Required(ErrorMessage = "Date Transferred is required.")]
        [DataType(DataType.Date)]
        [JsonPropertyName("DateTransferred")]
        [JsonConverter(typeof(DateStringConverter))] // Ensuring alignment with JSON schema reference
        public DateTime DateTransferred { get; set; }
    }

   
}