// File: Application/Dto/FirearmLineDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Application.Dto;

/* Item 1‑5 inside Section A */
public class FirearmLineDto
{
    public Guid? Id { get; set; }          // Optional on POST, required on PUT/PATCH

    [Required, StringLength(150)]
    public string ManufacturerImporter { get; set; } = default!;   // “Manufacturer and Importer (if any) or PMF”

    [StringLength(100)]
    public string? Model { get; set; }

    [Required, StringLength(50)]
    public string SerialNumber { get; set; } = default!;

    [Required, StringLength(20)]
    public string Type { get; set; } = default!;                    // Enums in schema (“Pistol”, “Rifle”, …)

    [Required, StringLength(50)]
    public string CaliberGauge { get; set; } = default!;
}
