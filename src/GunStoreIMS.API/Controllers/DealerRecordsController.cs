using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GunStoreIMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")] // Good practice to declare produces type
    public class DealerRecordsController : ControllerBase
    {
        private readonly IDealerRecordService _dealerService;
        private readonly IMapper _mapper;

        public DealerRecordsController(IDealerRecordService dealerService, IMapper mapper)
        {
            _dealerService = dealerService;
            _mapper = mapper;
        }

        /// <summary>Gets all dealer records, optionally filtering by active status.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DealerRecordDto>), 200)]
        public async Task<ActionResult<IEnumerable<DealerRecordDto>>> GetAll([FromQuery] bool? active = null)
        {
            var records = await _dealerService.GetAllDealerRecordsAsync(active);
            return Ok(_mapper.Map<IEnumerable<DealerRecordDto>>(records));
        }

        /// <summary>Gets a specific dealer record by its ID.</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DealerRecordDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DealerRecordDto>> GetById(Guid id)
        {
            var record = await _dealerService.GetDealerRecordByIdAsync(id);
            return record == null ? NotFound($"ID {id} not found.") : Ok(_mapper.Map<DealerRecordDto>(record));
        }

        /// <summary>Gets a specific dealer record by its FFL number.</summary>
        [HttpGet("ffl/{fflNumber}")]
        [ProducesResponseType(typeof(DealerRecordDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DealerRecordDto>> GetByFflNumber(string fflNumber)
        {
            var records = await _dealerService.GetRecordsByFflNumberAsync(fflNumber);
            var record = records.FirstOrDefault();
            return record == null ? NotFound($"FFL {fflNumber} not found.") : Ok(_mapper.Map<DealerRecordDto>(record));
        }

        /// <summary>Creates a new dealer record.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(DealerRecordDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<DealerRecordDto>> Create([FromBody] DealerRecordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<DealerRecord>(dto);
            var (createdRecord, error) = await _dealerService.AddDealerRecordAsync(entity);

            if (createdRecord == null)
            {
                if (error != null && error.StartsWith("Conflict:")) return Conflict(error);
                return BadRequest(error ?? "Failed to create record.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdRecord.Id }, _mapper.Map<DealerRecordDto>(createdRecord));
        }

        /// <summary>Updates an existing dealer record.</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DealerRecordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _dealerService.GetDealerRecordByIdAsync(id);
            if (existing == null) return NotFound($"ID {id} not found.");

            _mapper.Map(dto, existing); // Map updates onto existing
            existing.Id = id; // Ensure ID remains correct

            var (success, error) = await _dealerService.UpdateDealerRecordAsync(existing);

            return success ? NoContent() : BadRequest(error ?? "Failed to update record.");
        }

        /// <summary>Archives (deactivates) a dealer record.</summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Archive(Guid id)
        {
            var (success, error) = await _dealerService.ArchiveDealerRecordAsync(id);

            if (!success && error != null && error.StartsWith("Not Found:")) return NotFound(error);
            return success ? Ok($"Dealer record {id} archived.") : BadRequest(error ?? "Failed to archive record.");
        }

        /// <summary>Activates an archived dealer record.</summary>
        [HttpPost("{id:guid}/activate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Activate(Guid id)
        {
            var (success, error) = await _dealerService.ActivateDealerRecordAsync(id);

            if (!success && error != null && error.StartsWith("Not Found:")) return NotFound(error);
            return success ? Ok($"Dealer record {id} activated.") : BadRequest(error ?? "Failed to activate record.");
        }
    }
}