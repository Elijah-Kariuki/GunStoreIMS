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
    public class AcquisitionRecordsController : ControllerBase
    {
        private readonly FirearmsInventoryDB _db;
        private readonly IMapper _mapper;

        public AcquisitionRecordsController(FirearmsInventoryDB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ✅ GET: Retrieve all records for audits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcquisitionRecordDto>>> GetAll()
        {
            var records = await _db.AcquisitionRecords
                .OrderByDescending(r => r.AcquisitionDate) // Ensure latest first
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<AcquisitionRecordDto>>(records));
        }

        // ✅ GET: Retrieve a specific record for compliance audits
        [HttpGet("{id}")]
        public async Task<ActionResult<AcquisitionRecordDto>> GetById(Guid id)
        {
            var record = await _db.AcquisitionRecords.FindAsync(id);
            if (record == null) return NotFound();
            return Ok(_mapper.Map<AcquisitionRecordDto>(record));
        }

        // ✅ POST: Enforce ATF regulations & validation
        [HttpPost]
        public async Task<ActionResult<AcquisitionRecordDto>> Create(AcquisitionRecordDto dto)
        {
            // 🔹 Enforce ATF acquisition entry within **next business day**
            if (dto.AcquisitionDate < DateTime.UtcNow.AddDays(-1))
            {
                return BadRequest("ATF requires acquisitions to be logged by next business day.");
            }

            // 🔹 Prevent duplicate firearm entries
            var existingRecord = await _db.AcquisitionRecords
                .FirstOrDefaultAsync(r => r.SerialNumber == dto.SerialNumber);
            if (existingRecord != null)
            {
                return Conflict("Firearm with this serial number already exists in the acquisition records.");
            }

            var entity = _mapper.Map<AcquisitionRecord>(dto);
            entity.Id = Guid.NewGuid();

            _db.AcquisitionRecords.Add(entity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, _mapper.Map<AcquisitionRecordDto>(entity));
        }

        // ✅ PUT: Allow updates only within **ATF compliance constraints**
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AcquisitionRecordDto dto)
        {
            var existingRecord = await _db.AcquisitionRecords.FindAsync(id);
            if (existingRecord == null) return NotFound();

            // 🔹 Prevent backdating acquisitions outside compliance
            if (dto.AcquisitionDate < existingRecord.AcquisitionDate)
            {
                return BadRequest("Acquisition dates cannot be retroactively changed.");
            }

            _mapper.Map(dto, existingRecord);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: Restricted—ATF requires record retention
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var record = await _db.AcquisitionRecords.FindAsync(id);
            if (record == null) return NotFound();

            // 🔹 Ensure ATF **recordkeeping compliance** (Retain for at least 20 years)
            if (record.AcquisitionDate.AddYears(20) > DateTime.UtcNow)
            {
                return BadRequest("ATF requires acquisition records to be retained for at least 20 years.");
            }

            _db.AcquisitionRecords.Remove(record);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
