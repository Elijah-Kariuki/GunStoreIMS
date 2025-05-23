// File: Application/Dto/PermitDetailsDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Shared.Dto;

/* ======================================================================
   Permit Details DTO – Represents firearm permit information
   ==================================================================== */
public class PermitDetailsDto
{
    [StringLength(2)]
    public string? IssuingState { get; set; }

    public string? PermitType { get; set; }

    [DataType(DataType.Date)]
    public DateTime? IssuedDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ExpirationDate { get; set; }

    public string? PermitNumber { get; set; }
}
