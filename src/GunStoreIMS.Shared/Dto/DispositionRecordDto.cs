using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto
{
    public class DispositionRecordDto
    {
        public Guid? Id { get; set; } // Nullable for create, non-null for read

        [Required(ErrorMessage = "Transaction Date for disposition is required.")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Disposition FirearmType is required.")]
        [StringLength(50)]
        public string DispositionType { get; set; } = default!;

        [Required(ErrorMessage = "Transferee Name is required.")]
        [StringLength(200)]
        public string TransfereeName { get; set; } = default!;

        [StringLength(20)]
        public string? TransfereeFFLNumber { get; set; }

        [StringLength(150)] public string? TransfereeAddressLine1 { get; set; }
        [StringLength(150)] public string? TransfereeAddressLine2 { get; set; }
        [StringLength(100)] public string? TransfereeCity { get; set; }
        public string? TransfereeState { get; set; } // String for DTO, maps to USState enum
        [StringLength(15)] public string? TransfereeZip { get; set; }

        [Required(ErrorMessage = "Firearm ID for the disposed firearm is required.")]
        public Guid FirearmId { get; set; }

        // This serial number should match the linked Firearm's serial number.
        // It's recorded on the disposition line item per A&D regulations.
        [Required(ErrorMessage = "Serial Number of the disposed firearm is required.")]
        [StringLength(50)]
        public string SerialNumber { get; set; } = default!;

        [StringLength(50)]
        public string? Form4473TransactionNumber { get; set; } // NICS NTN or Form 4473 Serial

        [StringLength(500)]
        public string? Notes { get; set; }

        // You might also include the quantity of projectiles if tracking ammo with firearms.
        // public int? QuantityOfProjectiles { get; set; } // From your original DispositionRecord model
    }
}