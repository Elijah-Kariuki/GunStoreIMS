using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Shared.Enums;
using GunStoreIMS.Domain.Utilities;

namespace GunStoreIMS.Application.Services
{
    public class FirearmService : IFirearmService
    {
        private readonly FirearmsInventoryDB _db;
        private readonly IMapper _mapper;

        public FirearmService(FirearmsInventoryDB dbContext, IMapper mapper)
        {
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResultDto<FirearmLineDto>> QueryFirearmsAsync(
            string statusString, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            if (!Enum.TryParse<FirearmStatus>(statusString, true, out var status))
                return new PagedResultDto<FirearmLineDto>(new List<FirearmLineDto>(), 0, page, pageSize);

            var query = BusinessRules.WhereByStatus(_db.Firearms.AsQueryable(), status);
            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<FirearmLineDto>>(items);
            return new PagedResultDto<FirearmLineDto>(dtos, total, page, pageSize);
        }

        public async Task<FirearmDetailDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;
            var entity = await _db.Firearms.FindAsync(id);
            if (entity == null) return null;
            return _mapper.Map<FirearmDetailDto>(entity);
        }

        public async Task<OperationResult<FirearmDetailDto>> AcquireAsync(AcquisitionRecordDto dto)
        {
            if (dto == null)
                return OperationResult<FirearmDetailDto>.Fail("Acquisition DTO cannot be null.");

            if (!Enum.IsDefined(typeof(FirearmEnumType), dto.FirearmType))
                return OperationResult<FirearmDetailDto>.Fail($"Invalid FirearmType '{dto.FirearmType}' provided.");

            var vr = BusinessRules.CanAcquire(dto, _db);
            if (!vr.Succeeded)
                return OperationResult<FirearmDetailDto>.Fail(vr.ErrorMessage!);

            Firearm entity = dto.FirearmType switch
            {
                FirearmEnumType.Pistol => _mapper.Map<Pistol>(dto),
                FirearmEnumType.Revolver => _mapper.Map<Revolver>(dto),
                FirearmEnumType.Rifle => _mapper.Map<Rifle>(dto),
                FirearmEnumType.Shotgun => _mapper.Map<Shotgun>(dto),
                FirearmEnumType.Silencer => _mapper.Map<Silencer>(dto),
                _ => null!
            };

            if (entity == null)
                return OperationResult<FirearmDetailDto>.Fail($"Mapping for {dto.FirearmType} not implemented.");

            entity.Id = Guid.NewGuid();
            entity.CurrentStatus = FirearmStatus.InInventory;

            _db.Firearms.Add(entity);

            var acq = _mapper.Map<AcquisitionRecord>(dto);
            acq.Id = Guid.NewGuid();
            acq.FirearmId = entity.Id;
            _db.AcquisitionRecords.Add(acq);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult<FirearmDetailDto>
                    .Fail($"Failed to save acquisition: {ex.InnerException?.Message ?? ex.Message}");
            }

            var detail = _mapper.Map<FirearmDetailDto>(entity);
            return OperationResult<FirearmDetailDto>.Success(detail);
        }

        public async Task<bool> CorrectAsync(FirearmCorrectionDto dto)
        {
            if (dto == null || dto.FirearmId == Guid.Empty)
                return false;

            var entity = await _db.Firearms.FindAsync(dto.FirearmId);
            var vr = BusinessRules.CanCorrect(dto, entity);
            if (!vr.Succeeded)
                return false;

            _mapper.Map(dto, entity!);
            _db.Firearms.Update(entity!);

            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<OperationResult<FirearmDetailDto>> DisposeAsync(DispositionRecordDto dto)
        {
            if (dto == null)
                return OperationResult<FirearmDetailDto>.Fail("Disposition DTO cannot be null.");
            if (dto.FirearmId == Guid.Empty)
                return OperationResult<FirearmDetailDto>.Fail("Invalid Firearm ID.");

            var entity = await _db.Firearms.FindAsync(dto.FirearmId);
            var vr = BusinessRules.CanDispose(dto, entity);
            if (!vr.Succeeded)
                return OperationResult<FirearmDetailDto>.Fail(vr.ErrorMessage!);

            var disp = _mapper.Map<DispositionRecord>(dto);
            disp.Id = Guid.NewGuid();
            disp.FirearmId = entity!.Id;

            _db.DispositionRecords.Add(disp);
            _db.Firearms.Update(entity);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult<FirearmDetailDto>
                    .Fail($"Failed to save disposition: {ex.InnerException?.Message ?? ex.Message}");
            }

            var detail = _mapper.Map<FirearmDetailDto>(entity);
            return OperationResult<FirearmDetailDto>.Success(detail);
        }

        public async Task<OperationResult> ArchiveAsync(Guid id)
        {
            if (id == Guid.Empty)
                return OperationResult.Fail("Invalid Firearm ID.");

            var entity = await _db.Firearms.FindAsync(id);
            var vr = BusinessRules.CanArchive(entity);
            if (!vr.Succeeded)
                return OperationResult.Fail(vr.ErrorMessage!);

            entity!.CurrentStatus = FirearmStatus.Archived;
            _db.Firearms.Update(entity);

            try
            {
                await _db.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Fail($"Failed to save archival: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
