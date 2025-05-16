// Domain/Models/SectionC.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class SectionC
    {
        [Required, JsonPropertyName("BackgroundCheck")]
        public BackgroundCheck BackgroundCheck { get; set; } = new();
    }
}
