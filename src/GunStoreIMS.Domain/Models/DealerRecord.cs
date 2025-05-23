using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    public class DealerRecord
    {
        [Key]
        public Guid Id { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime RecordDate { get; set; }

        [Required]
        public bool IsAcquisition { get; set; }

        [Required, StringLength(100)]
        public string TradeName { get; set; } = default!;

        [Required, StringLength(200)]
        public string StreetAddress { get; set; } = default!;

        [StringLength(60)]
        public string? City { get; set; }

        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? Zip { get; set; }

        [Required, RegularExpression(@"^[0-9]-[0-9]{2}-[0-9]{5}$")]
        public string FFLNumber { get; set; } = default!;

        [Required, DataType(DataType.Date)]
        public DateTime LicenseExpirationDate { get; set; }
    }
}
