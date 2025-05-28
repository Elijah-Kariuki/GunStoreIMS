// File: Application/Dto/BuyerInfoDto.cs
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Shared.Enums;

namespace GunStoreIMS.Shared.Dto
{

    // ======================================================================
    //  BuyerInfo DTO (Used within SectionB)
    // ======================================================================
    public class BuyerInfoDto
    {
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(60)]
        [Display(Name = "9. Last Name")]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(60)]
        [Display(Name = "9. First Name")]
        public string FirstName { get; set; } = default!;

        [StringLength(60)]
        [Display(Name = "9. Middle Name")]
        public string? MiddleName { get; set; } // Nullable, 'NMN' handled by UI/Logic

        [Required]
        [Display(Name = "10. Current Residence Address")]
        public AddressDto ResidenceAddress { get; set; } = new();

        [Required]
        [Display(Name = "11. Place of Birth")]
        public PlaceOfBirthDto PlaceOfBirth { get; set; } = new();

        [Required]
        [Display(Name = "12. Height")]
        public HeightDto Height { get; set; } = new();

        [Required(ErrorMessage = "Weight is required.")]
        [Range(1, 999, ErrorMessage = "Weight must be between 1 and 999.")]
        [Display(Name = "13. Weight (lbs.)")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "Sex is required.")]
        [RegularExpression("^(Male|Female|Non-Binary)$", ErrorMessage = "Sex must be 'Male', 'Female', or 'Non-Binary'.")]
        [Display(Name = "14. Sex")]
        public string Sex { get; set; } = default!;

        [Required(ErrorMessage = "Birth Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "15. Birth Date")]
        public DateTime BirthDate { get; set; }

        [RegularExpression("^\\d{3}-?\\d{2}-?\\d{4}$", ErrorMessage = "Invalid SSN format.")]
        [Display(Name = "16. Social Security Number (Optional)")]
        public string? SSN { get; set; }

        [StringLength(50)]
        [Display(Name = "17. UPIN or AMD ID")]
        public string? UPINorAMD { get; set; }

        [Required(ErrorMessage = "Ethnicity is required.")]
        [RegularExpression("^(Hispanic or Latino|Not Hispanic or Latino)$", ErrorMessage = "Ethnicity must be 'Hispanic or Latino' or 'Not Hispanic or Latino'.")]
        [Display(Name = "18.a. Ethnicity")]
        public string Ethnicity { get; set; } = default!;

        [Required(ErrorMessage = "Race is required.")]
        [MinLength(1, ErrorMessage = "At least one Race must be selected.")]
        [Display(Name = "18.b. Race")]
        public List<string> Race { get; set; } = new(); // List should be validated against the enum values

        [Required(ErrorMessage = "Country of Citizenship is required.")]
        [MinLength(1, ErrorMessage = "At least one Country of Citizenship must be provided.")]
        [Display(Name = "19. Country of Citizenship")]
        public List<string> CountryOfCitizenship { get; set; } = new();

        [StringLength(50)]
        [Display(Name = "20. Alien/Admission Number")]
        public string? AlienOrAdmissionNumber { get; set; }
    }
}