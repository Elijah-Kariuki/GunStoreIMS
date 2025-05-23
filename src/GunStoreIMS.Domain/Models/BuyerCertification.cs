// Domain/Models/BuyerCertification.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class BuyerCertification
    {
        [Required, JsonPropertyName("Signature")]
        public string SignatureImage { get; set; } = default!;  // base64

        [Required, DataType(DataType.Date), JsonPropertyName("DateSigned")]
        public DateTime DateSigned { get; set; }
    }
}
