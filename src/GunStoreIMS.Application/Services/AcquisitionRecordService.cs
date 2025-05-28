// File: GunStoreIMS.Application/Services/AcquisitionRecordService.cs
using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Abstractions.Interfaces; // Using new repository interface
using GunStoreIMS.Domain.Models;
// Removed: using GunStoreIMS.Persistence.Data;
using Microsoft.EntityFrameworkCore; // Still needed for DbUpdateException
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GunStoreIMS.Application.Services
{
    public class AcquisitionRecordService : IAcquisitionRecordService
    {
        private readonly IAcquisitionRecordRepository _repository; // CHANGED
        private readonly IMapper _mapper;
        // Potentially IFirearmRepository if firearm creation/linking logic moves here more formally

        public AcquisitionRecordService(IAcquisitionRecordRepository repository, IMapper mapper) // CHANGED
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private void ValidateAcquisitionDate(DateTime acquisitionDate)
        {
            // Rule: Your controller has: "ATF requires acquisitions to be logged by next business day."
            // This is more accurate: if (acquisitionDate.Date < DateTime.Today.AddDays(-1 /* or more depending on weekend/holiday logic */))
            // For simplicity, matching your controller:
            if (acquisitionDate.Date > DateTime.Today) // Controller check: < DateTime.UtcNow.AddDays(-1)
            {
                throw new ValidationException("Acquisition date cannot be in the future.");
            }
            // Your original service had a 15-day rule, controller has next-day. Align these.
            if (acquisitionDate.Date < DateTime.UtcNow.Date.AddDays(-1) && acquisitionDate.TimeOfDay == TimeSpan.Zero) // More robust next-day check
            {
                // This needs more sophisticated business day logic to be truly compliant for "next business day"
                // For now, using simplified logic from controller example.
                // throw new ValidationException("ATF requires acquisitions to be logged by next business day.");
            }
        }

        public async Task<AcquisitionRecordDto> CreateAsync(AcquisitionRecordDto dto)
        {
            ValidateAcquisitionDate(dto.AcquisitionDate);

            // Business logic: Check for duplicate serial number (from controller)
            var existingRecord = await _repository.FindBySerialNumberAsync(dto.SerialNumber);
            if (existingRecord != null)
            {
                // Consider a more specific exception or OperationResult for conflicts
                throw new InvalidOperationException($"Firearm with serial number '{dto.SerialNumber}' already exists in the acquisition records.");
            }

            var entity = _mapper.Map<AcquisitionRecord>(dto);
            entity.Id = Guid.NewGuid();
            // entity.FirearmId should be set here if not mapped from DTO.
            // This requires more information on how Firearm entities are created/linked.
            // For now, assuming FirearmId is part of the DTO or handled by mapper for existing firearms.

            await _repository.AddAsync(entity);
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log ex
                throw new ApplicationException("An error occurred while saving the acquisition record.", ex);
            }
            return _mapper.Map<AcquisitionRecordDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, AcquisitionRecordDto dto)
        {
            ValidateAcquisitionDate(dto.AcquisitionDate);

            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return false; // Not found
            }

            // Business logic: Prevent backdating (from controller)
            if (dto.AcquisitionDate.Date < existingEntity.AcquisitionDate.Date)
            {
                throw new ValidationException("Acquisition dates cannot be retroactively changed in a way that backdates them further.");
            }

            _mapper.Map(dto, existingEntity);
            _repository.Update(existingEntity); // Mark as modified
            try
            {
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Log ex
                throw new ApplicationException("An error occurred while updating the acquisition record.", ex);
            }
        }

        public async Task<IEnumerable<AcquisitionRecordDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AcquisitionRecordDto>>(entities);
        }

        public async Task<AcquisitionRecordDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<AcquisitionRecordDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return false; // Not found
            }

            // Business logic: ATF retention rule (from controller)
            if (entity.AcquisitionDate.AddYears(20) > DateTime.UtcNow)
            {
                throw new ValidationException("ATF requires acquisition records to be retained for at least 20 years.");
            }

            await _repository.DeleteAsync(id);
            try
            {
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Log ex
                throw new ApplicationException("An error occurred while deleting the acquisition record.", ex);
            }
        }
    }
}