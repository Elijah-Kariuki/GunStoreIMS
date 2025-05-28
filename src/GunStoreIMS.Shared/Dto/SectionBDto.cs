// File: Application/Dto/SectionBDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{

    // ======================================================================
    //  Section B DTO – Transferee / Buyer Information (Main Container)
    // ======================================================================
    public class SectionBDto
    {
        /// <summary>
        /// Gets or sets the Buyer's Identification Information (Questions 9-20).
        /// </summary>
        [Required]
        public BuyerInfoDto BuyerInfo { get; set; } = new();

        /// <summary>
        /// Gets or sets the Buyer's Eligibility Answers (Question 21).
        /// </summary>
        [Required]
        public ProhibitorAnswersDto ProhibitorAnswers { get; set; } = new();

        /// <summary>
        /// Gets or sets the Buyer's Certification (Questions 22-23).
        /// </summary>
        [Required]
        public CertificationDto Certification { get; set; } = new();
    }
}