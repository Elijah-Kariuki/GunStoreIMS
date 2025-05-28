using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class BuyerInfo // Corresponds to $defs/BuyerInfo
    {
        /// <summary>
        /// 9. Last Name
        /// </summary>
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(60, ErrorMessage = "Last Name cannot exceed 60 characters.")]
        [JsonPropertyName("LastName")]
        public string LastName { get; set; } = default!;

        /// <summary>
        /// 9. First Name
        /// </summary>
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(60, ErrorMessage = "First Name cannot exceed 60 characters.")]
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// 9. Middle Name ('NMN' if none)
        /// </summary>
        [StringLength(60, ErrorMessage = "Middle Name cannot exceed 60 characters.")]
        [JsonPropertyName("MiddleName")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// 10. Current Residence Address
        /// </summary>
        [Required(ErrorMessage = "Residence Address is required.")]
        [JsonPropertyName("ResidenceAddress")]
        public Address ResidenceAddress { get; set; } = default!;

        /// <summary>
        /// Place of Birth
        /// </summary>
        [Required(ErrorMessage = "Place of Birth is required.")]
        [JsonPropertyName("PlaceOfBirth")]
        public PlaceOfBirth PlaceOfBirth { get; set; } = default!;

        /// <summary>
        /// Height
        /// </summary>
        [Required(ErrorMessage = "Height is required.")]
        [JsonPropertyName("Height")]
        public Height Height { get; set; } = default!;

        /// <summary>
        /// 13. Weight (lbs.)
        /// </summary>
        [Required(ErrorMessage = "Weight is required.")]
        [Range(1, 999, ErrorMessage = "Weight must be between 1 and 999 lbs.")]
        [JsonPropertyName("Weight")]
        public int Weight { get; set; }

        /// <summary>
        /// 14. Sex
        /// </summary>
        [Required(ErrorMessage = "Sex is required.")]
        [JsonPropertyName("Sex")]
        [EnumDataType(typeof(SexType))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SexType Sex { get; set; }

        /// <summary>
        /// 15. Birth Date
        /// </summary>
        [Required(ErrorMessage = "Birth Date is required.")]
        [DataType(DataType.Date)]
        [JsonPropertyName("BirthDate")]
        [JsonConverter(typeof(DateStringConverter))]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 16. SSN (Optional)
        /// </summary>
        [JsonPropertyName("SSN")]
        [RegularExpression(@"^\d{3}-?\d{2}-?\d{4}$", ErrorMessage = "SSN must follow XXX-XX-XXXX format.")]
        public string? SSN { get; set; }

        /// <summary>
        /// 17. UPIN/AMD ID
        /// </summary>
        [StringLength(50, ErrorMessage = "UPIN/AMD ID cannot exceed 50 characters.")]
        [JsonPropertyName("UPINorAMD")]
        public string? UPINorAMD { get; set; }

        /// <summary>
        /// 18.a. Ethnicity
        /// </summary>
        [Required(ErrorMessage = "Ethnicity is required.")]
        [JsonPropertyName("Ethnicity")]
        [EnumDataType(typeof(EthnicityType))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EthnicityType Ethnicity { get; set; }

        /// <summary>
        /// 18.b. Race
        /// </summary>
        [Required(ErrorMessage = "Race is required.")]
        [JsonPropertyName("Race")]
        public List<RaceType> Race { get; set; } = new();

        /// <summary>
        /// 19. Country of Citizenship
        /// </summary>
        [Required(ErrorMessage = "Country of Citizenship is required.")]
        [JsonPropertyName("CountryOfCitizenship")]
        public List<string> CountryOfCitizenship { get; set; } = new();

        /// <summary>
        /// 20. Alien/Admission Number (if applicable)
        /// </summary>
        [StringLength(50, ErrorMessage = "Alien/Admission Number cannot exceed 50 characters.")]
        [JsonPropertyName("AlienOrAdmissionNumber")]
        public string? AlienOrAdmissionNumber { get; set; }
    }

    public enum SexType { Male, Female, NonBinary }
    public enum EthnicityType { HispanicOrLatino, NotHispanicOrLatino }
    public enum RaceType { AmericanIndianOrAlaskaNative, Asian, BlackOrAfricanAmerican, NativeHawaiianOrOtherPacificIslander, White }
}