using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunStoreIMS.Domain.Models
{
    // ======================================================================
    //  Firearm Line Item Model (Related Table)
    // ======================================================================

    public class Form4473FirearmLine
    {
        [Key]
        public Guid Id { get; set; } // Primary Key for this line item

        [Required]
        public Guid Form4473RecordId { get; set; } // Foreign Key

        [ForeignKey(nameof(Form4473RecordId))]
        public virtual Form4473Record Form4473Record { get; set; } = default!;

        [Required, StringLength(150)]
        public string ManufacturerImporter { get; set; } = default!;
        [Required, StringLength(60)]
        public string Model { get; set; } = default!;
        [Required, StringLength(60)]
        public string SerialNumber { get; set; } = default!;
        [Required, StringLength(50)]
        public string Type { get; set; } = default!;
        [Required, StringLength(30)]
        public string CaliberGauge { get; set; } = default!;
    }
}
