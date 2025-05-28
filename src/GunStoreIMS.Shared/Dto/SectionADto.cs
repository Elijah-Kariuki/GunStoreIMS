using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Required for List

namespace GunStoreIMS.Shared.Dto
{

    /// <summary>
    /// Represents Section A (Firearm(s) Description) of ATF Form 4473 (Rev. Aug 2023).
    /// Corresponds to JSON Schema: https://yourgunstore.com/schemas/atf-4473-v3.1.0.json
    /// </summary>
    public class SectionADto
    {
        /// <summary>
        /// Gets or sets the list of firearms to be transferred (Lines 1-5).
        /// </summary>
        [Required(ErrorMessage = "At least one firearm must be listed.")]
        [MinLength(1, ErrorMessage = "At least one firearm must be listed.")]
        [MaxLength(29, ErrorMessage = "Cannot exceed 29 firearms (including continuation sheets).")]
        [Display(Name = "Firearms to Transfer (1-5)")]
        public List<FirearmLineDto> Firearms { get; set; } = new();

        /// <summary>
        /// Gets or sets the total number of firearms, spelled out (Question 6).
        /// </summary>
        [Required(ErrorMessage = "Total Number of Firearms is required.")]
        [StringLength(100, ErrorMessage = "Total Number cannot exceed 100 characters.")]
        [RegularExpression(
            "^(one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|[A-Za-z -]+)$",
            ErrorMessage = "Total Number must be spelled out (e.g., 'one', 'two').")]
        [Display(Name = "6. Total Number of Firearms")]
        public string TotalNumber { get; set; } = default!;

        /// <summary>
        /// Gets or sets whether any part of this transaction is a pawn redemption (Question 7).
        /// Defaults to false.
        /// </summary>
        [Display(Name = "7. Pawn Redemption?")]
        public bool IsPawnRedemption { get; set; }

        /// <summary>
        /// Gets or sets the line numbers corresponding to pawn redemptions (Question 7).
        /// This should be required via business logic or custom validation if IsPawnRedemption is true.
        /// </summary>
        [StringLength(100, ErrorMessage = "Pawn Redemption Lines cannot exceed 100 characters.")]
        [Display(Name = "7. Pawn Redemption Line Number(s)")]
        public string? PawnRedemptionLines { get; set; } // Nullable, as it's optional

        /// <summary>
        /// Gets or sets whether this transaction facilitates a private party transfer (Question 8).
        /// Defaults to false.
        /// </summary>
        [Display(Name = "8. Facilitating Private-Party Transfer?")]
        public bool IsPrivatePartyTransfer { get; set; }

    
    }

   
}