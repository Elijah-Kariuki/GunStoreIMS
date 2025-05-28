using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned] // Mark as Complex/Owned Type if used with EF Core
    public class NonImmigrantExceptionDocs
    {
        [Required]
        public NonImmigrantExceptionType ExceptionType { get; set; }

        [StringLength(100)]
        public string? DocumentIdentifier { get; set; } // e.g., License Number

        [StringLength(100)]
        public string? IssuingAuthority { get; set; } // e.g., "VA DGIF", "DOJ"

        [StringLength(200)]
        [Required]
        public string Description { get; set; } = string.Empty; // Maps to the 200-char field

        // Optional: Link to a scanned document
        public Guid? ScannedDocumentId { get; set; }
        // public virtual Document? ScannedDocument { get; set; }
    }
}
