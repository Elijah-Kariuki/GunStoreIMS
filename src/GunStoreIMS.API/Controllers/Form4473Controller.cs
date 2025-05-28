// API/Controllers/Form4473Controller.cs
using AutoMapper;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GunStoreIMS.Shared.Dto;


[ApiController]
[Route("api/[controller]")]
public class Form4473Controller : ControllerBase
{
    private readonly FirearmsInventoryDB _context;
    private readonly IMapper _mapper;

    public Form4473Controller(FirearmsInventoryDB context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Form4473RecordDto>>> GetAll()
    {
        var entities = await _context.Form4473Records.ToListAsync();
        // ensure each entity has SectionA populated
        entities.ForEach(e => {
            e.MapFirearmLinesToSectionAForSerialization();
        });
        var dtos = _mapper.Map<List<Form4473RecordDto>>(entities);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Form4473RecordDto>> GetById(Guid id)
    {
        var entity = await _context.Form4473Records.FindAsync(id);
        if (entity == null) return NotFound();

        entity.MapFirearmLinesToSectionAForSerialization();
        var dto = _mapper.Map<Form4473RecordDto>(entity);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<Form4473RecordDto>> Create(Form4473RecordDto dto)
    {
        var entity = _mapper.Map<Form4473Record>(dto);
        entity.Id = Guid.NewGuid();
        _context.Form4473Records.Add(entity);
        await _context.SaveChangesAsync();

        // prepare return DTO
        entity.MapFirearmLinesToSectionAForSerialization();
        var resultDto = _mapper.Map<Form4473RecordDto>(entity);

        return CreatedAtAction(nameof(GetById),
            new { id = resultDto.Id },
            resultDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Form4473RecordDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var entity = _mapper.Map<Form4473Record>(dto);
        _context.Entry(entity).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Form4473Records.Any(e => e.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _context.Form4473Records.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Form4473Records.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
