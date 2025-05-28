// File: GunStoreIMS.Application/Services/DispositionRecordService.cs
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
    public class DispositionRecordService : IDispositionRecordService
    {
        private readonly IDispositionRecordRepository _repository; // CHANGED
        private readonly IMapper _mapper;
        // Potentially IFirearmRepository if firearm status update logic is more complex

        public DispositionRecordService(IDispositionRecordRepository repository, IMapper mapper) // CHANGED
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private void ValidateDispositionDate(DateTime transactionDate)
        {
            // Rule: Your controller has: "ATF requires dispositions to be recorded within 7 days."
            // if (transactionDate.Date < DateTime.UtcNow.Date.AddDays(-7))
            // {
            //     throw new ValidationException("ATF requires dispositions to be recorded within 7 days.");
            // }
            // Original service check:
            if (transactionDate.Date > DateTime.UtcNow.Date.AddDays(1))
            {
                throw new ValidationException("Disposition date cannot be unreasonably in the future.");
            }
            // Align with controller's 7-day rule for past dates.
            if (transactionDate.Date < DateTime.UtcNow.Date.AddDays(-7))
            {
                throw new ValidationException("ATF requires dispositions to be recorded within 7 days.");
            }
        }

        public async Task<DispositionRecordDto> CreateAsync(DispositionRecordDto dto)
        {
            ValidateDispositionDate(dto.TransactionDate);

            var entity = _mapper.Map<DispositionRecord>(dto);
            entity.Id = Guid.NewGuid();
            // Ensure entity.FirearmId is correctly set/mapped.
            // Logic to update the associated Firearm's status to 'Disposed' should be here
            // or in a IFirearmService call. This is critical.

            await _repository.AddAsync(entity);
            try
            {
                // TODO: Add logic here to fetch the Firearm by entity.FirearmId
                // and update its CurrentStatus to FirearmStatus.Disposed,
                // and set its DateOfDisposition.
                // var firearmToUpdate = await _firearmRepository.GetByIdAsync(entity.FirearmId);
                // if (firearmToUpdate != null) {
                //     firearmToUpdate.CurrentStatus = FirearmStatus.Disposed;
                //     firearmToUpdate.DateOfDisposition = entity.TransactionDate;
                //     _firearmRepository.Update(firearmToUpdate); // Assuming FirearmRepository exists
                // }
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log ex
                throw new ApplicationException("An error occurred while saving the disposition record.", ex);
            }
            return _mapper.Map<DispositionRecordDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, DispositionRecordDto dto)
        {
            ValidateDispositionDate(dto.TransactionDate);

            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return false; // Not found
            }

            // Business logic: Prevent backdating (from controller)
            if (dto.TransactionDate.Date < existingEntity.TransactionDate.Date)
            {
                throw new ValidationException("Disposition record dates cannot be retroactively modified to an earlier date.");
            }

            _mapper.Map(dto, existingEntity);
            _repository.Update(existingEntity);
            try
            {
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Log ex
                throw new ApplicationException("An error occurred while updating the disposition record.", ex);
            }
        }

        public async Task<IEnumerable<DispositionRecordDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DispositionRecordDto>>(entities);
        }

        public async Task<DispositionRecordDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<DispositionRecordDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return false; // Not found
            }

            // Business logic: ATF retention rule (from controller)
            if (entity.TransactionDate.AddYears(20) > DateTime.UtcNow)
            {
                throw new ValidationException("ATF requires disposition records to be retained for at least 20 years.");
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
                throw new ApplicationException("An error occurred while deleting the disposition record.", ex);
            }
        }
    }
}