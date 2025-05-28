// File: Application/Dto/Form4473RecordDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  Section E DTO – Dealer Certification (Main Container)
    // ======================================================================
    public class SectionEDto
    {
        /// <summary>
        /// Gets or sets optional notes for licensee use (Question 32).
        /// </summary>
        [StringLength(500)]
        [Display(Name = "32. For Use by Licensee (Optional Notes)")]
        public string? DealerNotes { get; set; }

        /// <summary>
        /// Gets or sets the Dealer's Information (Question 33).
        /// </summary>
        [Required(ErrorMessage = "Dealer Information is required.")]
        [Display(Name = "33. Dealer Information")]
        public DealerInfoDto DealerInfo { get; set; } = new();

        /// <summary>
        /// Gets or sets the Transferor's/Seller's Certification (Questions 34-36).
        /// </summary>
        [Required(ErrorMessage = "Transferor Certification is required.")]
        [Display(Name = "34-36. Transferor Certification")]
        public TransferorCertificationDto TransferorSignature { get; set; } = new();
    }
}