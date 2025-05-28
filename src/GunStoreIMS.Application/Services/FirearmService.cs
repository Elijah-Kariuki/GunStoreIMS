// File: GunStoreIMS.Application/Services/FirearmService.cs
using AutoMapper;
using GunStoreIMS.Abstractions.Interfaces; // Use IFirearmRepository
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Domain.Utilities;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Shared.Enums;
using Microsoft.EntityFrameworkCore; // For DbUpdateException
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GunStoreIMS.Application.Services;

namespace GunStoreIMS.Application.Services
{
    public class FirearmService : IFirearmService
    {
        private readonly IFirearmRepository _firearmRepository; // CHANGED
        private readonly IAcquisitionRecordRepository _acquisitionRecordRepository; // If creating AcquisitionRecord here
        private readonly IDispositionRecordRepository _dispositionRecordRepository; // If creating DispositionRecord here
        private readonly IMapper _mapper;
        private readonly BusinessRules _businessRules; // Assuming this contains CanAcquire, CanDispose etc.
                                                                            // Or inject specific rule classes like FirearmBusinessRulesManager

        public FirearmService(
            IFirearmRepository firearmRepository, // CHANGED
            IAcquisitionRecordRepository acquisitionRecordRepository, // ADDED
            IDispositionRecordRepository dispositionRecordRepository, // ADDED
            IMapper mapper)
        {
            _firearmRepository = firearmRepository ?? throw new ArgumentNullException(nameof(firearmRepository));
            _acquisitionRecordRepository = acquisitionRecordRepository ?? throw new ArgumentNullException(nameof(acquisitionRecordRepository));
            _dispositionRecordRepository = dispositionRecordRepository ?? throw new ArgumentNullException(nameof(dispositionRecordRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _businessRules = new Application.Services.BusinessRules(); // Or inject if it has dependencies
        }

        public async Task<PagedResultDto<FirearmLineDto>> QueryFirearmsAsync(
            string statusString, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            if (!Enum.TryParse<FirearmStatus>(statusString, true, out var status))
            {
                // FIX: Use 4-arg constructor and set ErrorMessage property
                return new PagedResultDto<FirearmLineDto>(new List<FirearmLineDto>(), 0, page, pageSize)
                {
                    ErrorMessage = "Invalid status string."
                };
            }
            // Query logic moved to repository
            var (items, total) = await _firearmRepository.QueryAsync(status, page, pageSize);

            var dtos = _mapper.Map<List<FirearmLineDto>>(items);
            return new PagedResultDto<FirearmLineDto>(dtos, total, page, pageSize);
        }

        public async Task<FirearmDetailDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;
            var entity = await _firearmRepository.GetByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<FirearmDetailDto>(entity);
        }

        public async Task<OperationResult<FirearmDetailDto>> AcquireAsync(AcquisitionRecordDto acquisitionDto)
        {
            if (acquisitionDto == null)
                return OperationResult<FirearmDetailDto>.Fail("Acquisition DTO cannot be null.");

            // Potentially use a dedicated FirearmCreateDto for the firearm part
            if (!Enum.IsDefined(typeof(FirearmEnumType), acquisitionDto.FirearmType)) // FirearmType is on AcquisitionRecordDto
                return OperationResult<FirearmDetailDto>.Fail($"Invalid FirearmType '{acquisitionDto.FirearmType}' provided.");

            // Check for uniqueness (Example: Assuming AcquisitionRecordDto has Manufacturer, Model, ImporterName)
            // This check is more robust in the repository: ExistsBySerialNumberAsync
            bool exists = await _firearmRepository.ExistsBySerialNumberAsync(
                acquisitionDto.SerialNumber, // Assuming SerialNumber on DTO is for the Firearm
                acquisitionDto.Manufacturer, // Assuming these are on AcquisitionRecordDto
                acquisitionDto.Model,        // Assuming these are on AcquisitionRecordDto
                acquisitionDto.ImporterName); // Assuming these are on AcquisitionRecordDto

            if (exists)
            {
                return OperationResult<FirearmDetailDto>.Fail($"Firearm with serial number '{acquisitionDto.SerialNumber}' by manufacturer '{acquisitionDto.Manufacturer}' and model '{acquisitionDto.Model}' already exists.");
            }

            // TODO: Replace with proper mapping for Firearm from a FirearmCreateDto or parts of AcquisitionRecordDto
            Firearm firearmEntity = acquisitionDto.FirearmType switch
            {
                FirearmEnumType.Pistol => _mapper.Map<Pistol>(acquisitionDto), // Need specific mappings
                FirearmEnumType.Revolver => _mapper.Map<Revolver>(acquisitionDto),
                FirearmEnumType.Rifle => _mapper.Map<Rifle>(acquisitionDto),
                FirearmEnumType.Shotgun => _mapper.Map<Shotgun>(acquisitionDto),
                FirearmEnumType.Silencer => _mapper.Map<Silencer>(acquisitionDto),
                // ... other types ...
                _ => _mapper.Map<Firearm>(acquisitionDto) // Fallback or throw
            };

            if (firearmEntity == null)
                return OperationResult<FirearmDetailDto>.Fail($"Mapping for {acquisitionDto.FirearmType} not implemented or failed.");

            firearmEntity.Id = Guid.NewGuid();
            firearmEntity.CurrentStatus = FirearmStatus.InInventory;
            firearmEntity.InitialAcquisitionDate = acquisitionDto.AcquisitionDate; // Set initial acquisition date

            await _firearmRepository.AddAsync(firearmEntity);

            var acqRecordEntity = _mapper.Map<AcquisitionRecord>(acquisitionDto);
            acqRecordEntity.Id = Guid.NewGuid();
            acqRecordEntity.FirearmId = firearmEntity.Id; // Link to the new firearm
            // Ensure AcquisitionRecord.SerialNumber is set correctly if it's distinct from Firearm.SerialNumber
            acqRecordEntity.SerialNumber = firearmEntity.SerialNumber; // Typically they are the same for the first acquisition.

            await _acquisitionRecordRepository.AddAsync(acqRecordEntity);

            try
            {
                await _firearmRepository.SaveChangesAsync(); // This will save both firearm and its acquisition record if context is shared
            }
            catch (DbUpdateException ex)
            {
                // Consider specific error handling or logging
                return OperationResult<FirearmDetailDto>
                    .Fail($"Failed to save acquisition: {ex.InnerException?.Message ?? ex.Message}");
            }

            var detailDto = _mapper.Map<FirearmDetailDto>(firearmEntity);
            return OperationResult<FirearmDetailDto>.Success(detailDto);
        }

        public async Task<bool> CorrectAsync(FirearmCorrectionDto dto)
        {
            if (dto == null || dto.FirearmId == Guid.Empty)
                return false;

            var entity = await _firearmRepository.GetByIdAsync(dto.FirearmId);
            if (entity == null) return false; // Firearm not found

            // Example validation from BusinessRules (adapt as needed)
            // var vr = _businessRules.CanCorrect(dto, entity);
            // if (!vr.Succeeded) return false; // Or throw, or return OperationResult

            _mapper.Map(dto, entity); // Apply corrections
            _firearmRepository.Update(entity);

            try
            {
                await _firearmRepository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                // Log error
                return false;
            }
        }

        public async Task<OperationResult<FirearmDetailDto>> DisposeAsync(DispositionRecordDto dispositionDto)
        {
            if (dispositionDto == null || dispositionDto.FirearmId == Guid.Empty)
                return OperationResult<FirearmDetailDto>.Fail("Invalid disposition data or Firearm ID.");

            var firearmEntity = await _firearmRepository.GetByIdAsync(dispositionDto.FirearmId);
            if (firearmEntity == null)
                return OperationResult<FirearmDetailDto>.Fail("Firearm to dispose not found.");

            // Example from BusinessRules (adapt as needed)
            // var vr = _businessRules.CanDispose(dispositionDto, firearmEntity);
            // if (!vr.Succeeded) return OperationResult<FirearmDetailDto>.Fail(vr.ErrorMessage!);
            if (firearmEntity.CurrentStatus != FirearmStatus.InInventory && firearmEntity.CurrentStatus != FirearmStatus.InRepair) // Simplified check
            {
                return OperationResult<FirearmDetailDto>.Fail($"Firearm is not in a disposable state. Current status: {firearmEntity.CurrentStatus}.");
            }


            var dispRecordEntity = _mapper.Map<DispositionRecord>(dispositionDto);
            dispRecordEntity.Id = Guid.NewGuid();
            // FirearmId is already on dispositionDto and should be mapped.
            dispRecordEntity.SerialNumber = firearmEntity.SerialNumber; // Record SN on disposition line.

            await _dispositionRecordRepository.AddAsync(dispRecordEntity);

            firearmEntity.CurrentStatus = FirearmStatus.Disposed;
            firearmEntity.DateOfDisposition = dispRecordEntity.TransactionDate;
            _firearmRepository.Update(firearmEntity);

            try
            {
                await _firearmRepository.SaveChangesAsync(); // Save both disposition and firearm update
            }
            catch (DbUpdateException ex)
            {
                return OperationResult<FirearmDetailDto>
                    .Fail($"Failed to save disposition: {ex.InnerException?.Message ?? ex.Message}");
            }

            var detailDto = _mapper.Map<FirearmDetailDto>(firearmEntity);
            return OperationResult<FirearmDetailDto>.Success(detailDto);
        }

        public async Task<OperationResult> ArchiveAsync(Guid id)
        {
            if (id == Guid.Empty)
                return OperationResult.Fail("Invalid Firearm ID.");

            var entity = await _firearmRepository.GetByIdAsync(id);
            if (entity == null)
                return OperationResult.Fail("Firearm to archive not found.");

            // Example from BusinessRules (adapt as needed)
            // var vr = _businessRules.CanArchive(entity);
            // if (!vr.Succeeded) return OperationResult.Fail(vr.ErrorMessage!);
            if (entity.CurrentStatus != FirearmStatus.Disposed)
            {
                return OperationResult.Fail("Firearm must be disposed before it can be archived.");
            }

            entity.CurrentStatus = FirearmStatus.Archived;
            _firearmRepository.Update(entity);

            try
            {
                await _firearmRepository.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Fail($"Failed to archive firearm: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}