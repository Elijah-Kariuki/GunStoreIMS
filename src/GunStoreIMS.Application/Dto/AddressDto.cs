// File: Application/Dto/AddressDto.cs
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Domain.Enums;

namespace GunStoreIMS.Application.Dto;

public class AddressDto
{
    [Required, StringLength(100)]
    public string Street { get; set; } = default!;

    [Required, StringLength(60)]
    public string City { get; set; } = default!;

    [StringLength(60)]
    public string? County { get; set; }

    [Required]
    public USState State { get; set; }

    [Required, StringLength(10)]
    public string Zip { get; set; } = default!;

    public bool? ResideInCityLimits { get; set; }
}
