// File: Application/Dto/ProhibitorAnswersDto.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto;

/* -- B.2  Eligibility questions (21 a‑n) ------------------------------- */
public class ProhibitorAnswersDto
{
    [Required] public bool? IsActualTransfereeBuyer { get; set; }
    [Required] public bool? WillDisposeToFelony { get; set; }
    [Required] public bool? UnderIndictment { get; set; }
    [Required] public bool? EverConvictedFelony { get; set; }
    [Required] public bool? FugitiveFromJustice { get; set; }
    [Required] public bool? UnlawfulUserOfControlledSubstance { get; set; }
    [Required] public bool? AdjudicatedMentallyDefective { get; set; }
    [Required] public bool? DishonorableDischarge { get; set; }
    [Required] public bool? SubjectToRestrainingOrder { get; set; }
    [Required] public bool? ConvictedMisdemeanorDomesticViolence { get; set; }
    [Required] public bool? RenouncedUSCitizenship { get; set; }
    [Required] public bool? AlienIllegally { get; set; }
    [Required] public bool? NonImmigrantVisa { get; set; }
    public bool? NonImmigrantVisaException { get; set; }
    [Required] public bool? WillDisposeToProhibitedPerson { get; set; }
}
