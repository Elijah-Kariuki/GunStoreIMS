using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a document stored in the system, potentially linked to various records.
    /// </summary>
    public class Document
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "File name is required.")]
        [StringLength(260, ErrorMessage = "File name cannot exceed 260 characters.")] // Max path length considerations
        public string FileName { get; set; } = default!;

        [StringLength(100, ErrorMessage = "Content type cannot exceed 100 characters.")]
        public string? ContentType { get; set; } // e.g., "application/pdf", "image/jpeg"

        /// <summary>
        /// Could be a path to a file on a file system, a blob storage URI, or even binary data if stored in DB (not recommended for large files).
        /// </summary>
        [Required]
        public string StorageReference { get; set; } = default!;

        public long? FileSizeInBytes { get; set; }

        public DateTime DateUploadedUtc { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // Add any other relevant properties like UploaderUserId, etc.

        public Document()
        {
            Id = Guid.NewGuid();
            DateUploadedUtc = DateTime.UtcNow;
        }
    }
}