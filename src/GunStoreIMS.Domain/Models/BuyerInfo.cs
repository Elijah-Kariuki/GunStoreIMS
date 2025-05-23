// Domain/Models/BuyerInfo.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class BuyerInfo
    {
        [Required, StringLength(60), JsonPropertyName("LastName")]
        public string LastName { get; set; } = default!;

        [Required, StringLength(60), JsonPropertyName("FirstName")]
        public string FirstName { get; set; } = default!;

        [StringLength(60), JsonPropertyName("MiddleName")]
        public string? MiddleName { get; set; }

        [Required, JsonPropertyName("ResidenceAddress")]
        public Address ResidenceAddress { get; set; } = new();

        [Required, JsonPropertyName("PlaceOfBirth")]
        public PlaceOfBirth PlaceOfBirth { get; set; } = new();

        [Required, RegularExpression(@"^[0-9]{1,2}'[0-9]{1,2}\""$"), JsonPropertyName("Height")]
        public string Height { get; set; } = default!;

        [Required, Range(1, 999), JsonPropertyName("Weight")]
        public int Weight { get; set; }

        [Required, JsonPropertyName("Sex")]
        public string Sex { get; set; } = default!;  // could be an enum

        [Required, DataType(DataType.Date), JsonPropertyName("BirthDate")]
        public DateTime BirthDate { get; set; }

        [RegularExpression(@"^\d{3}-?\d{2}-?\d{4}$"), JsonPropertyName("SSN")]
        public string? SSN { get; set; }

        [JsonPropertyName("UPINorAMD")]
        public string? UPINorAMD { get; set; }

        [Required, JsonPropertyName("Ethnicity")]
        public string Ethnicity { get; set; } = default!;

        [Required, MinLength(1), JsonPropertyName("Race")]
        public List<string> Race { get; set; } = new();

        [Required, MinLength(1), JsonPropertyName("CountryOfCitizenship")]
        public List<string> CountryOfCitizenship { get; set; } = new();

        [JsonPropertyName("AlienNumber")]
        public string? AlienNumber { get; set; }
    }

   
}
