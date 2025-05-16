// File: Application/Dto/BuyerCertificationDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Application.Dto;

/* -- B.3  Buyer signature & date -------------------------------------- */
public class BuyerCertificationDto
{
    /// <summary>Base‑64 PNG/JPEG captured by front‑end signature pad.</summary>
    [Required]
    public string SignatureImage { get; set; } = default!;

    [Required]
    public DateTime DateSigned { get; set; }
}
