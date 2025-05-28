using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{

    // ======================================================================
    //  Address DTO (Used within BuyerInfo)
    // ======================================================================
    public class AddressDto
    {
        [Required(ErrorMessage = "Street Address is required.")]
        [StringLength(100)]
        [Display(Name = "Number and Street Address")]
        // NOTE: P.O. Box validation should be handled in application logic
        // or a custom validation attribute based on schema's intent.
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(60)]
        [Display(Name = "City")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "State is required.")]
        [RegularExpression("^[A-Z]{2}$", ErrorMessage = "State must be a 2-letter code.")]
        [Display(Name = "State")]
        public string State { get; set; } = default!;

        [Required(ErrorMessage = "ZIP Code is required.")]
        [RegularExpression("^\\d{5}(?:-\\d{4})?$", ErrorMessage = "ZIP Code must be 5 or 9 digits.")]
        [Display(Name = "ZIP Code")]
        public string Zip { get; set; } = default!;

        [Required(ErrorMessage = "County/Parish/Borough is required.")]
        [StringLength(60)]
        [Display(Name = "County/Parish/Borough")]
        public string County { get; set; } = default!;

        [Required(ErrorMessage = "Reside in City Limits? must be answered.")]
        [RegularExpression("^(Yes|No|Unknown)$", ErrorMessage = "Must be 'Yes', 'No', or 'Unknown'.")]
        [Display(Name = "Reside in City Limits?")]
        public string ResideInCityLimits { get; set; } = default!; // "Yes", "No", or "Unknown"
    }
}