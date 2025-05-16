// File: Application/Dto/PlaceOfBirthDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Application.Dto;

public class PlaceOfBirthDto
{
    [StringLength(60)]
    public string? USCity { get; set; }

    [StringLength(2)]
    public string? USState { get; set; }

    [Required, StringLength(60)]
    public string Country { get; set; } = default!;
}
