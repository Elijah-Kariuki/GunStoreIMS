// File: Application/Dto/Form4473RecordDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto;

/// <summary>
/// Root record for ATF Form 4473 (Sections A–E).  
/// Only Sections A–B are sent from the public UI; C–E can be gated
/// behind dealer/FFL roles or handled in admin‑only DTOs.
/// </summary>
public class Form4473RecordDto
{
    public Guid? Id { get; set; }

    [StringLength(50)]
    public string? TransferorsTransactionNumber { get; set; }

    // ---------- Section A ----------
    [Required]
    public SectionADto SectionA { get; set; } = new();

    // ---------- Section B ----------
    [Required]
    public SectionBDto SectionB { get; set; } = new();

    // (Optional) Section C–E DTOs for back‑office
    // public SectionCDto? SectionC { get; set; }
    // public SectionDDto? SectionD { get; set; }
    // public SectionEDto? SectionE { get; set; }
}
