using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Dto
{
    // ======================================================================
    //  TransferorCertification DTO (Used within SectionE) - Q34-36
    // ======================================================================
    public class TransferorCertificationDto
    {
        /// <summary>
        /// Gets or sets the Transferor's/Seller's Printed Name (Question 34).
        /// </summary>
        [Required(ErrorMessage = "Transferor Name is required.")]
        [StringLength(100)]
        [Display(Name = "34. Transferor’s/Seller’s Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Base64 encoded signature image for the Transferor/Seller (Question 35).
        /// </summary>
        [Required(ErrorMessage = "Transferor Signature is required.")]
        [Display(Name = "35. Transferor’s/Seller’s Signature")]
        public string SignatureImage { get; set; } = default!; // Base64 Encoded String

        /// <summary>
        /// Gets or sets the Date Transferred (Question 36).
        /// </summary>
        [Required(ErrorMessage = "Date Transferred is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "36. Date Transferred")]
        public DateTime DateTransferred { get; set; }
    }

}
