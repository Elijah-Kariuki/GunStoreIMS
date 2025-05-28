// File: GunStoreIMS.Application.Services/BusinessRules.cs
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Enums; // Assuming FirearmStatus is here
using GunStoreIMS.Persistence.Data;
using GunStoreIMS.Domain.Utilities; // ✅ Use shared OperationResult
using System.Linq; // For IQueryable

namespace GunStoreIMS.Application.Services
{
    public  class BusinessRules
    {
        // Fix for CS0708 and CA1822: Mark the method as static since it does not access instance data.  
        public  OperationResult<bool> CanAcquire(AcquisitionRecordDto dto, FirearmsInventoryDB dbContext)
        {
            if (dto == null)
                return OperationResult<bool>.Fail("Acquisition data cannot be null.");

            // Corrected: Check for required fields directly from AcquisitionRecordDto  
            if (string.IsNullOrWhiteSpace(dto.SerialNumber))
                return OperationResult<bool>.Fail("Firearm serial number is required for acquisition.");

            // Example: Check for serial number uniqueness  
            if (dbContext.Firearms.Any(f => f.SerialNumber == dto.SerialNumber))
            {
                return OperationResult<bool>.Fail($"Firearm with serial number '{dto.SerialNumber}' already exists.");
            }

            // Add other acquisition-specific business logic here  
            return OperationResult<bool>.Success(true);
        }
    }
}