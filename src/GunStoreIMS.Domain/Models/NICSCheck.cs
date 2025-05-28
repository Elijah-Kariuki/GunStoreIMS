using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GunStoreIMS.Domain.Utilities;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Domain.Models
{
    [Owned]
    public class NICSCheck : IValidatableObject // Corresponds to $defs/NICSCheck
    {
        /// <summary>
        /// 28. No NICS Required Because
        /// </summary>
        [JsonPropertyName("NoNicsRequiredReason")]
        [EnumDataType(typeof(NoNicsRequiredReason))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NoNicsRequiredReason? NoNicsRequiredReason { get; set; }

        /// <summary>
        /// 29. Permit Details (If applicable)
        /// </summary>
        [JsonPropertyName("PermitDetails")]
        public PermitDetails? PermitDetails { get; set; }

        /// <summary>
        /// 27.a. Date to NICS
        /// </summary>
        [DataType(DataType.Date)]
        [JsonPropertyName("DateContacted")]
        [JsonConverter(typeof(DateStringConverter))] // Ensuring alignment with JSON schema
        public DateTime? DateContacted { get; set; }

        /// <summary>
        /// 27.b. NICS/STN Transaction #
        /// </summary>
        [StringLength(50)]
        [JsonPropertyName("TransactionNumber")]
        public string? TransactionNumber { get; set; }

        /// <summary>
        /// 27.c. Initial Response
        /// </summary>
        [JsonPropertyName("InitialResponse")]
        [EnumDataType(typeof(NicsResponseType))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NicsResponseType? InitialResponse { get; set; }

        /// <summary>
        /// 27.c. MDI / Brady Transfer Date
        /// </summary>
        [DataType(DataType.Date)]
        [JsonPropertyName("BradyTransferDate")]
        [JsonConverter(typeof(DateStringConverter))]
        public DateTime? BradyTransferDate { get; set; }

        /// <summary>
        /// 27.d. Later Response
        /// </summary>
        [JsonPropertyName("LaterResponse")]
        [EnumDataType(typeof(NicsResponseType))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NicsResponseType? LaterResponse { get; set; }

        /// <summary>
        /// 27.d. Date
        /// </summary>
        [DataType(DataType.Date)]
        [JsonPropertyName("LaterResponseDate")]
        [JsonConverter(typeof(DateStringConverter))]
        public DateTime? LaterResponseDate { get; set; }

        /// <summary>
        /// 27.e. Post-Transfer Response
        /// </summary>
        [JsonPropertyName("PostTransferResponse")]
        [EnumDataType(typeof(NicsResponseType))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NicsResponseType? PostTransferResponse { get; set; }

        /// <summary>
        /// 27.e. Date
        /// </summary>
        [DataType(DataType.Date)]
        [JsonPropertyName("PostTransferResponseDate")]
        [JsonConverter(typeof(DateStringConverter))]
        public DateTime? PostTransferResponseDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool isExempt = NoNicsRequiredReason.HasValue;
            bool isContacted = DateContacted.HasValue && InitialResponse.HasValue;

            if (!isExempt && !isContacted)
            {
                yield return new ValidationResult(
                    "Either a No NICS Required reason or NICS check details (Date & Initial Response) must be provided.",
                    new[] { nameof(NoNicsRequiredReason), nameof(DateContacted), nameof(InitialResponse) });
            }
            if (isExempt && isContacted)
            {
                yield return new ValidationResult(
                    "Cannot provide both a No NICS Required reason and NICS check details.",
                    new[] { nameof(NoNicsRequiredReason), nameof(DateContacted), nameof(InitialResponse) });
            }
            if (NoNicsRequiredReason == GunStoreIMS.Shared.Enums.NoNicsRequiredReason.PermitQualified)
            {
                if (PermitDetails == null)
                {
                    yield return new ValidationResult(
                        "Permit Details must be provided when No NICS Required reason is Permit-Qualified.",
                        new[] { nameof(PermitDetails) });
                }
            }
            else if (PermitDetails != null)
            {
                yield return new ValidationResult(
                    "Permit Details should only be provided when No NICS Required reason is Permit-Qualified.",
                    new[] { nameof(PermitDetails) });
            }
        }
    }
}