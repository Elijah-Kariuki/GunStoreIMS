// Domain/Models/TransferorSignature.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class TransferorSignature
    {
        [Required, StringLength(100), JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [Required, JsonPropertyName("SignatureImage")]
        public string SignatureImage { get; set; } = default!;  // base64

        [Required, DataType(DataType.Date), JsonPropertyName("DateTransferred")]
        public DateTime DateTransferred { get; set; }
    }
}
