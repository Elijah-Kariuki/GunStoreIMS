// Domain/Models/SectionE.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionE
    {
        [Required, JsonPropertyName("DealerInfo")]
        public DealerRecord DealerInfo { get; set; } = new();

        [Required, JsonPropertyName("TransferorSignature")]
        public TransferorSignature TransferorSignature { get; set; } = new();
    }
}
