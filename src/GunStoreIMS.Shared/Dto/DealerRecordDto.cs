using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{
    public class DealerRecordDto
    {
        public Guid? Id { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime RecordDate { get; set; }

        [Required]
        public bool IsAcquisition { get; set; }

        [Required, StringLength(100)]
        public string? DealerName { get; set; }

        [Required, StringLength(200)]
        public string? StreetAddress { get; set; }

        [StringLength(60)]
        public string? City { get; set; }

        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? Zip { get; set; }

        [Required, RegularExpression(@"^[0-9]-[0-9]{2}-[0-9]{5}$")]
        public string? FFLNumber { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime LicenseExpirationDate { get; set; }

        // …any other client-facing notes, etc…
    }
}
