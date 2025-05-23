// GunStoreIMS.Application.Services.DispositionRecordService.cs
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
    public class DispositionRecordService : IDispositionRecordService
    {
        private readonly FirearmsInventoryDB _db;
        private readonly IMapper _mapper;

        public DispositionRecordService(FirearmsInventoryDB db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private void ValidateDispositionDate(DateTime transactionDate)
        {
            // Ensure transaction date is not unreasonably in the future
            if (transactionDate.Date > DateTime.UtcNow.AddDays(1))
            {
                throw new ValidationException("Disposition date cannot be unreasonably in the future.");
            }
        }

        public async Task<DispositionRecordDto> CreateAsync(DispositionRecordDto dto)
        {
            ValidateDispositionDate(dto.TransactionDate);

            var entity = _mapper.Map<DispositionRecord>(dto);
            entity.Id = Guid.NewGuid(); // Assign new ID

            _db.DispositionRecords.Add(entity);
            await _db.SaveChangesAsync();
            return _mapper.Map<DispositionRecordDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, DispositionRecordDto dto)
        {
            ValidateDispositionDate(dto.TransactionDate);

            var existingEntity = await _db.DispositionRecords.FirstOrDefaultAsync(dr => dr.Id == id);
            if (existingEntity == null)
            {
                return false; // Not found
            }

            _mapper.Map(dto, existingEntity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DispositionRecordDto>> GetAllAsync()
        {
            var entities = await _db.DispositionRecords.ToListAsync();
            return _mapper.Map<IEnumerable<DispositionRecordDto>>(entities);
        }

        public async Task<DispositionRecordDto?> GetByIdAsync(Guid id)
        {
            var entity = await _db.DispositionRecords.FirstOrDefaultAsync(dr => dr.Id == id);
            return entity == null ? null : _mapper.Map<DispositionRecordDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.DispositionRecords.FindAsync(id);
            if (entity == null)
            {
                return false; // Not found
            }

            _db.DispositionRecords.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}