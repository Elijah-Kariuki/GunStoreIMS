using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto;

/// <summary>
/// Represents Section D (Recertification) of ATF Form 4473 (Rev. Aug 2023).
/// This section is only completed if the firearm transfer occurs on a
/// different day than the Section B certification date.
/// Corresponds to JSON Schema: https://yourgunstore.com/schemas/atf-4473-v3.1.0.json
/// </summary>
public class SectionDDto
{
    /// <summary>
    /// Gets or sets the buyer's recertification details (Questions 30-31).
    /// This will be null if Section D is not applicable or not completed.
    /// </summary>
    [Display(Name = "Buyer Recertification (30-31)")]
    public CertificationDto? BuyerRecertification { get; set; }
}

/*
// ======================================================================
//  Certification DTO (Defined previously, shown for context)
// ======================================================================
public class CertificationDto
{
    /// <summary>
    /// Gets or sets the Base64 encoded signature image.
    /// </summary>
    [Required(ErrorMessage = "Signature is required.")]
    [Display(Name = "Signature")]
    public string Signature { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date the certification was signed.
    /// </summary>
    [Required(ErrorMessage = "Certification Date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Certification Date")]
    public DateTime DateSigned { get; set; }
}
*/