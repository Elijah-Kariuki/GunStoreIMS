// File: API/Controllers/FirearmsController.cs
using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.API.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GunStoreIMS.Application.Services;
using GunStoreIMS.Domain.Interfaces;

namespace GunStoreIMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirearmsController : ControllerBase
    {
        private readonly IFirearmService _svc;

        public FirearmsController(IFirearmService svc)
            => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<FirearmLineDto>>> GetAll(
            [FromQuery] string status = "InInventory",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25)
        {
            var paged = await _svc.QueryFirearmsAsync(status, page, pageSize);
            return Ok(paged);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FirearmDetailDto>> GetById(Guid id)
        {
            var detail = await _svc.GetByIdAsync(id);
            if (detail == null) return NotFound($"Firearm {id} not found.");
            return Ok(detail);
        }

        [HttpPost("acquire")]
        public async Task<ActionResult<FirearmDetailDto>> Acquire([FromBody] AcquisitionRecordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _svc.AcquireAsync(dto);
            if (!result.Succeeded) return Conflict(result.ErrorMessage);

            // result.Entity is now your FirearmDetailDto
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Entity!.Id },
                result.Entity
            );
        }

        [HttpPut("{id}/correct")]
        [ValidateRouteAndBodyId("id", "dto")]
        public async Task<IActionResult> Correct(Guid id, [FromBody] FirearmCorrectionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // CorrectAsync now returns a bool, not an OperationResult
            var success = await _svc.CorrectAsync(dto);
            if (!success)
                return BadRequest("Failed to correct firearm record. Ensure the firearm exists and the correction is valid.");

            return NoContent();
        }

        [HttpPost("{id}/dispose")]
        [ValidateRouteAndBodyId("id", "dto")]
        public async Task<IActionResult> Dispose(Guid id, [FromBody] DispositionRecordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _svc.DisposeAsync(dto);
            if (!result.Succeeded) return BadRequest(result.ErrorMessage);
            return Ok(result.Entity);
        }

        [HttpDelete("{id}/archive")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var result = await _svc.ArchiveAsync(id);
            if (!result.Succeeded) return BadRequest(result.ErrorMessage);
            return Ok("Archived");
        }
    }
}
