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
    public class DealerRecordsController : ControllerBase
    {
        private readonly FirearmsInventoryDB _db;
        private readonly IMapper _mapper;

        public DealerRecordsController(FirearmsInventoryDB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ✅ GET: Retrieve all dealer records for audits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealerRecordDto>>> GetAll()
        {
            var records = await _db.DealerRecords
                .OrderByDescending(r => r.RecordDate) // Ensure latest first
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DealerRecordDto>>(records));
        }

        // ✅ GET: Retrieve a specific dealer record by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DealerRecordDto>> GetById(Guid id)
        {
            var record = await _db.DealerRecords.FindAsync(id);
            if (record == null) return NotFound();
            return Ok(_mapper.Map<DealerRecordDto>(record));
        }

        // ✅ POST: Add a dealer acquisition or disposition (ATF compliance)
        [HttpPost]
        public async Task<ActionResult<DealerRecordDto>> Create(DealerRecordDto dto)
        {
            // 🔹 Enforce ATF timing rules for acquisitions & dispositions
            if (dto.IsAcquisition && dto.RecordDate < DateTime.UtcNow.AddDays(-1))
            {
                return BadRequest("ATF requires acquisitions to be logged by the next business day.");
            }
            else if (!dto.IsAcquisition && dto.RecordDate < DateTime.UtcNow.AddDays(-7))
            {
                return BadRequest("Dispositions must be recorded within 7 days.");
            }

            var entity = _mapper.Map<DealerRecord>(dto);
            entity.Id = Guid.NewGuid();

            _db.DealerRecords.Add(entity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, _mapper.Map<DealerRecordDto>(entity));
        }

        // ✅ PUT: Allow updates only within **ATF compliance constraints**
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, DealerRecordDto dto)
        {
            var existingRecord = await _db.DealerRecords.FindAsync(id);
            if (existingRecord == null) return NotFound();

            // 🔹 Prevent retroactive record changes
            if (dto.RecordDate < existingRecord.RecordDate)
            {
                return BadRequest("Dealer record dates cannot be retroactively modified.");
            }

            _mapper.Map(dto, existingRecord);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: **Restricted**—ATF requires record retention
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var record = await _db.DealerRecords.FindAsync(id);
            if (record == null) return NotFound();

            // 🔹 Enforce ATF **recordkeeping compliance** (retain for at least 20 years)
            if (record.RecordDate.AddYears(20) > DateTime.UtcNow)
            {
                return BadRequest("ATF requires dealer records to be retained for at least 20 years.");
            }

            _db.DealerRecords.Remove(record);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
