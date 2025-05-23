// File: Application/Dto/SectionBDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto;

/* ======================================================================
   Section B  – Transferee / Buyer Information
   ==================================================================== */
public class SectionBDto
{
    [Required]
    public BuyerInfoDto BuyerInfo { get; set; } = new();

    [Required]
    public ProhibitorAnswersDto ProhibitorAnswers { get; set; } = new();

    [Required]
    public BuyerCertificationDto BuyerCertification { get; set; } = new();
}
