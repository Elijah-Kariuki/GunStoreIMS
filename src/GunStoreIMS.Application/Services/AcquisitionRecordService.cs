// GunStoreIMS.Application.Services.AcquisitionRecordService.cs
using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Interfaces;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data; // Correct namespace for FirearmsInventoryDB
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // For ValidationException
using System.Threading.Tasks;

namespace GunStoreIMS.Application.Services
{
    public class AcquisitionRecordService : IAcquisitionRecordService
    {
        private readonly FirearmsInventoryDB _db; // Updated to use FirearmsInventoryDB
        private readonly IMapper _mapper;

        public AcquisitionRecordService(FirearmsInventoryDB db, IMapper mapper) // Updated constructor
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private void ValidateAcquisitionDate(DateTime acquisitionDate)
        {
            // Rule: Must be recorded within 15 days of the event (i.e., not older than 15 days ago)
            if (acquisitionDate.Date < DateTime.Today.AddDays(-15))
            {
                throw new ValidationException("Acquisition must be recorded within 15 days of the event.");
            }
            // Rule: Cannot be in the future
            if (acquisitionDate.Date > DateTime.Today)
            {
                throw new ValidationException("Acquisition date cannot be in the future.");
            }
        }

        public async Task<AcquisitionRecordDto> CreateAsync(AcquisitionRecordDto dto)
        {
            ValidateAcquisitionDate(dto.AcquisitionDate);

            // In a real scenario, you would first find or create the associated Firearm entity
            // based on Manufacturer, Model, SerialNumber, etc., from the DTO.
            // For this example, we assume the DTO might carry a FirearmId or that the mapping handles it.
            // The current AcquisitionRecord DTO doesn't have individual firearm fields,
            // and AcquisitionRecord domain model has FirearmId.
            // This implies the Firearm entity must exist or be created separately, and its ID provided.
            // Let's assume the DTO has enough info to create/find a Firearm, or FirearmId is set.

            var entity = _mapper.Map<AcquisitionRecord>(dto);
            entity.Id = Guid.NewGuid(); // Ensure new Guid for new record

            // If your DTO contains firearm details (Manufacturer, Model, SerialNumber etc.)
            // and you need to link to an existing Firearm or create a new one:
            // 1. Try to find an existing Firearm based on unique identifiers.
            //    var firearm = await _db.Firearms.FirstOrDefaultAsync(f => f.SerialNumber == dto.SerialNumber && f.Manufacturer == dto.Manufacturer /* ... more criteria */);
            // 2. If not found, create a new Firearm entity.
            //    if (firearm == null) { /* create new Firearm, map from DTO, add to _db.Firearms */ }
            // 3. Set entity.FirearmId = firearm.Id;
            // This logic is complex and depends on your exact DTO and business rules for firearm creation/linking.
            // The current DTO (AcquisitionRecordDto) doesn't seem to carry these individual firearm fields.
            // It only has a SerialNumber field that seems to belong to the AcquisitionRecord itself,
            // which is fine for A&D book keeping, but the link to a master Firearm entity is key.

            // For the provided DTO structure, which is flat, the mapping to AcquisitionRecord
            // would need to handle how FirearmId is populated if it's not directly in the DTO.
            // If dto.Id is for the AcquisitionRecord itself, it's ignored by mapping profile for create.

            _db.AcquisitionRecords.Add(entity);
            await _db.SaveChangesAsync();
            return _mapper.Map<AcquisitionRecordDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, AcquisitionRecordDto dto)
        {
            ValidateAcquisitionDate(dto.AcquisitionDate);

            var existingEntity = await _db.AcquisitionRecords
                                          // .Include(ar => ar.Firearm) // Include related entities if needed for update logic
                                          .FirstOrDefaultAsync(ar => ar.Id == id);

            if (existingEntity == null)
            {
                return false; // Not found
            }

            // The AutoMapper profile should ignore mapping the Id from DTO to entity.
            _mapper.Map(dto, existingEntity);
            // existingEntity.Id should remain 'id' from the parameter.
            // If dto.Id is present and meant to be the firearmId or some other linked ID,
            // that mapping needs to be explicit or handled differently.

            // _db.AcquisitionRecords.Update(existingEntity); // EF Core tracks changes, explicit Update often not needed if entity is tracked.
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AcquisitionRecordDto>> GetAllAsync()
        {
            var entities = await _db.AcquisitionRecords
                                    // .Include(ar => ar.Firearm) // If you want to include Firearm details
                                    .ToListAsync();
            return _mapper.Map<IEnumerable<AcquisitionRecordDto>>(entities);
        }

        public async Task<AcquisitionRecordDto?> GetByIdAsync(Guid id)
        {
            var entity = await _db.AcquisitionRecords
                                  // .Include(ar => ar.Firearm) // If you want to include Firearm details
                                  .FirstOrDefaultAsync(ar => ar.Id == id);

            return entity == null ? null : _mapper.Map<AcquisitionRecordDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.AcquisitionRecords.FindAsync(id);
            if (entity == null)
            {
                return false; // Not found
            }

            _db.AcquisitionRecords.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
