using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class Height // Corresponds to $defs/Height
    {
        /// <summary>
        /// 12. Height in Feet (1–8)
        /// </summary>
        [Required(ErrorMessage = "Height in feet is required.")]
        [Range(1, 8, ErrorMessage = "Height in feet must be between 1 and 8.")]
        [JsonPropertyName("Feet")]
        public int Feet { get; set; }

        /// <summary>
        /// 12. Height in Inches (0–11)
        /// </summary>
        [Required(ErrorMessage = "Height in inches is required.")]
        [Range(0, 11, ErrorMessage = "Height in inches must be between 0 and 11.")]
        [JsonPropertyName("Inches")]
        public int Inches { get; set; }
    }
}