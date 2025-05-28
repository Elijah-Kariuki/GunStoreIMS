using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto;

// ======================================================================
//  DealerInfo DTO (Used within SectionE) - Q33
// ======================================================================
public class DealerInfoDto
{
    /// <summary>
    /// Gets or sets the Trade/Corporate Name (Question 33).
    /// </summary>
    [Required(ErrorMessage = "Trade Name is required.")]
    [StringLength(100)]
    [Display(Name = "33. Trade Name")]
    public string TradeName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Street Address (Question 33).
    /// </summary>
    [Required(ErrorMessage = "Street Address is required.")]
    [StringLength(100)]
    [Display(Name = "33. Street Address")]
    public string StreetAddress { get; set; } = default!;

    /// <summary>
    /// Gets or sets the City (Question 33).
    /// </summary>
    [Required(ErrorMessage = "City is required.")]
    [StringLength(60)]
    [Display(Name = "33. City")]
    public string City { get; set; } = default!;

    /// <summary>
    /// Gets or sets the State (Question 33).
    /// </summary>
    [Required(ErrorMessage = "State is required.")]
    [RegularExpression("^[A-Z]{2}$", ErrorMessage = "State must be a 2-letter code.")]
    [Display(Name = "33. State")]
    public string State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the ZIP Code (Question 33).
    /// </summary>
    [Required(ErrorMessage = "ZIP Code is required.")]
    [RegularExpression("^\\d{5}(?:-\\d{4})?$", ErrorMessage = "ZIP Code must be 5 or 9 digits.")]
    [Display(Name = "33. ZIP Code")]
    public string Zip { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Federal Firearms License Number (Question 33).
    /// </summary>
    [Required(ErrorMessage = "FFL Number is required.")]
    [RegularExpression("^[0-9]-[0-9]{2}-[0-9]{5}$", ErrorMessage = "FFL Number must be in X-XX-XXXXX format.")]
    [Display(Name = "33. FFL Number")]
    public string FFLNumber { get; set; } = default!;
}
