using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionE // Corresponds to $defs/SectionE
    {
        /// <summary>
        /// 32. For Use by Licensee (Optional Notes)
        /// </summary>
        [StringLength(500, ErrorMessage = "Dealer notes cannot exceed 500 characters.")]
        [JsonPropertyName("DealerNotes")]
        public string? DealerNotes { get; set; }

        /// <summary>
        /// Dealer Information
        /// </summary>
        [Required(ErrorMessage = "DealerInfo is required.")]
        [JsonPropertyName("DealerInfo")]
        public DealerInfo DealerInfo { get; set; } = default!;

        /// <summary>
        /// Transferor Certification Signature
        /// </summary>
        [Required(ErrorMessage = "TransferorSignature is required.")]
        [JsonPropertyName("TransferorSignature")]
        public TransferorCertification TransferorSignature { get; set; } = default!;
    }
}