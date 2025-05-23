// File: GunStoreIMS.Application.Services/BusinessRules.cs
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Enums; // Assuming FirearmStatus is here
using GunStoreIMS.Persistence.Data;
using GunStoreIMS.Domain.Utilities; // ✅ Use shared OperationResult
using System.Linq; // For IQueryable

namespace GunStoreIMS.Application.Services
{
    public static class BusinessRules
    {
        // Fix for CS1061: Replace the incorrect reference to FirearmDetails with the correct property SerialNumber directly from AcquisitionRecordDto.

        public static OperationResult<bool> CanAcquire(AcquisitionRecordDto dto, FirearmsInventoryDB dbContext)
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

        public static OperationResult<bool> CanCorrect(FirearmCorrectionDto dto, Firearm? firearm) // firearm can be null if ID in DTO is invalid
        {
            if (dto == null)
                return OperationResult<bool>.Fail("Correction data cannot be null.");
            if (firearm == null)
                return OperationResult<bool>.Fail("Firearm to correct not found.");

            // Add additional business logic here
            // e.g., check if the firearm is in a state that allows correction
            // if (firearm.CurrentStatus == FirearmStatus.Archived)
            //    return OperationResult<bool>.Fail("Cannot correct an archived firearm.");

            return OperationResult<bool>.Success(true);
        }

        public static OperationResult CanDispose(DispositionRecordDto dto, Firearm? firearm)
        {
            if (dto == null)
                return OperationResult.Fail("Disposition data cannot be null.");
            if (firearm == null)
                return OperationResult.Fail("Firearm to dispose not found.");

            // Example: Check if firearm is in a disposable state
            if (firearm.CurrentStatus != FirearmStatus.InInventory && firearm.CurrentStatus != FirearmStatus.InRepair) // Example states
            {
                return OperationResult.Fail($"Firearm is not in a disposable state. Current status: {firearm.CurrentStatus}.");
            }
            // Add additional business logic here
            return OperationResult.Success();
        }

        public static OperationResult CanArchive(Firearm? firearm)
        {
            if (firearm == null)
                return OperationResult.Fail("Firearm to archive not found.");

            // Example: Check if firearm is in an archivable state (e.g., must be disposed first)
            if (firearm.CurrentStatus != FirearmStatus.Disposed)
            {
                return OperationResult.Fail("Firearm must be disposed before it can be archived.");
            }
            // Add additional archiving validation rules here (e.g., retention period)
            return OperationResult.Success();
        }

        public static IQueryable<Firearm> WhereByStatus(this IQueryable<Firearm> query, FirearmStatus status)
        {
            return query.Where(f => f.CurrentStatus == status);
        }
    }
}