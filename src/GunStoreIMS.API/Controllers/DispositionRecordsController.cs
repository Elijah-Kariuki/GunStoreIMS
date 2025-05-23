using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DispositionRecordsController : ControllerBase
    {
        private readonly FirearmsInventoryDB _db;
        private readonly IMapper _mapper;

        public DispositionRecordsController(FirearmsInventoryDB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ✅ GET: Retrieve all disposition records for audits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DispositionRecordDto>>> GetAll()
        {
            var records = await _db.DispositionRecords
                .OrderByDescending(r => r.TransactionDate) // Ensure latest first
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DispositionRecordDto>>(records));
        }

        // ✅ GET: Retrieve a specific disposition record by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DispositionRecordDto>> GetById(Guid id)
        {
            var record = await _db.DispositionRecords.FindAsync(id);
            if (record == null) return NotFound();
            return Ok(_mapper.Map<DispositionRecordDto>(record));
        }

        // ✅ POST: Add a disposition record (ATF compliance)
        [HttpPost]
        public async Task<ActionResult<DispositionRecordDto>> Create(DispositionRecordDto dto)
        {
            // 🔹 Enforce ATF timing rules (dispositions must be recorded within 7 days)
            if (dto.TransactionDate < DateTime.UtcNow.AddDays(-7))
            {
                return BadRequest("ATF requires dispositions to be recorded within 7 days.");
            }

            var entity = _mapper.Map<DispositionRecord>(dto);
            entity.Id = Guid.NewGuid();

            _db.DispositionRecords.Add(entity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, _mapper.Map<DispositionRecordDto>(entity));
        }

        // ✅ PUT: Allow updates only within **ATF compliance constraints**
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, DispositionRecordDto dto)
        {
            var existingRecord = await _db.DispositionRecords.FindAsync(id);
            if (existingRecord == null) return NotFound();

            // 🔹 Prevent retroactive record changes
            if (dto.TransactionDate < existingRecord.TransactionDate)
            {
                return BadRequest("Disposition record dates cannot be retroactively modified.");
            }

            _mapper.Map(dto, existingRecord);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: **Restricted**—ATF requires record retention
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var record = await _db.DispositionRecords.FindAsync(id);
            if (record == null) return NotFound();

            // 🔹 Enforce ATF **recordkeeping compliance** (retain for at least 20 years)
            if (record.TransactionDate.AddYears(20) > DateTime.UtcNow)
            {
                return BadRequest("ATF requires disposition records to be retained for at least 20 years.");
            }

            _db.DispositionRecords.Remove(record);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
