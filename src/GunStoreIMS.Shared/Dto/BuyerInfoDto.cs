// File: Application/Dto/BuyerInfoDto.cs
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto;

/* -- B.1  Buyer identification block ---------------------------------- */
public class BuyerInfoDto
{
    [Required, StringLength(60)]
    public string LastName { get; set; } = default!;

    [Required, StringLength(60)]
    public string FirstName { get; set; } = default!;

    [StringLength(60)]
    public string? MiddleName { get; set; }

    [Required]
    public AddressDto ResidenceAddress { get; set; } = new();

    [Required]
    public PlaceOfBirthDto PlaceOfBirth { get; set; } = new();

    /// <example>5'11"</example>
    [Required, RegularExpression(@"^[0-9]{1,2}'[0-9]{1,2}\""$")]
    public string Height { get; set; } = default!;

    [Required, Range(1, 999)]
    public int Weight { get; set; }

    [Required]
    public SexOption Sex { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [StringLength(11)]
    public string? SSN { get; set; }

    [StringLength(25)]
    public string? UpinOrAmdId { get; set; }

    [Required]
    public EthnicityOption Ethnicity { get; set; }

    [Required]
    public List<RaceOption> Race { get; set; } = new();

    [Required]
    public List<string> CountryOfCitizenship { get; set; } = new();

    [StringLength(50)]
    public string? AlienNumber { get; set; }
}
