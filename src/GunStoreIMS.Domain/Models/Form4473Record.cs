// Domain/Models/Form4473Record.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Domain.Models;

public class Form4473Record
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string? TransferorsTransactionNumber { get; set; }

    [Required]
    [JsonPropertyName("SectionA")]
    public SectionA SectionA { get; set; } = new();

    [Required]
    [JsonPropertyName("SectionB")]
    public SectionB SectionB { get; set; } = new();

    // Sections C–E omitted for brevity
    [Required]
    [JsonPropertyName("SectionC")]
    public SectionC SectionC { get; set; } = new();
    [Required]
    [JsonPropertyName("SectionD")]
    public SectionD SectionD { get; set; } = new();
    [Required]
    [JsonPropertyName("SectionE")]
    public SectionE SectionE { get; set; } = new();

    public ICollection<Form4473FirearmLine> Form4473FirearmLines { get; set; } = new List<Form4473FirearmLine>();

}
