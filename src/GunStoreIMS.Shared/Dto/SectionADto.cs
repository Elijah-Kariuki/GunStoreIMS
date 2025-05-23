using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto;

/* ===========================================================
   Section A DTO – travels to / from the React front-end
   ===========================================================*/
public class SectionADto
{
    [Required, StringLength(50)]
    [RegularExpression(
        "^(one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|[A-Za-z ]+)$",
        ErrorMessage = "Value must be the word form (e.g. \"one\", \"two\").")]
    public string TotalNumber { get; set; } = default!;

    public bool IsPawnRedemption { get; set; }

    public bool IsPrivatePartyTransfer { get; set; }

    /// <summary>Injected when 4 + firearms require a 5300.9A sheet</summary>
    public bool? ContinuationSheet { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(29)]
    public List<FirearmLineDto> Firearms { get; set; } = new();
}
