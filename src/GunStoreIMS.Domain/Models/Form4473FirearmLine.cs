using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// One line (items 1-5) from Section A of ATF Form 4473.
    /// If the dealer uses a continuation sheet, additional rows are stored here.
    /// </summary>
    public class Form4473FirearmLine
    {
        [Key]
        public Guid Id { get; set; }

        /* FK back to parent record ---------------------------------------- */
        [Required]
        public Guid Form4473RecordId { get; set; }

        [ForeignKey(nameof(Form4473RecordId))]
        public virtual Form4473Record Form4473Record { get; set; } = default!;

        /* 1. Manufacturer & Importer (or PMF) ------------------------------ */
        [Required, StringLength(100)]              // schema maxLength: 100
        public string ManufacturerImporter { get; set; } = default!;

        /* 2. Model (if designated) ---------------------------------------- */
        [StringLength(60)]                         // schema maxLength: 60
        public string? Model { get; set; }

        /* 3. Serial Number ------------------------------------------------- */
        [Required, StringLength(60)]               // schema maxLength: 60
        public string SerialNumber { get; set; } = default!;

        /* 4. FirearmType (pistol, rifle, etc.) ----------------------------------- */
        [Required, StringLength(20)]               // shorter length; enum later
        // Consider: public FirearmLineType FirearmType { get; set; }
        public string FirearmType { get; set; } = default!;

        /* 5. Caliber or Gauge --------------------------------------------- */
        [Required, StringLength(30)]               // schema maxLength: 30
        public string CaliberGauge { get; set; } = default!;
    }
}
