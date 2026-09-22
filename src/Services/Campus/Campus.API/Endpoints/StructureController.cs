using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Campus.Domain.Entities;
using Campus.Domain.ValueObjects;
using Campus.Application.DTOs;
using Campus.Infraestructure.Persistence;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/structures")]
public class StructureController : ControllerBase
{
    private readonly CampusDbContext _context;

    public StructureController(CampusDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StructureDto>>> GetAll(
        [FromQuery] Guid? campusId,
        [FromQuery] Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var query = _context.Structures.AsNoTracking();

        if (campusId.HasValue)
        {
            query = query.Where(s => s.CampusId == campusId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(s => s.CategoryId == categoryId.Value);
        }

        var structures = await query
            .Select(s => new StructureDto(
                s.Id,
                s.CampusId,
                s.CategoryId,
                s.Name,
                s.Coordinates.Latitude,
                s.Coordinates.Longitude
            ))
            .ToListAsync(cancellationToken);

        return Ok(structures);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StructureDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var structure = await _context.Structures
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (structure is null)
        {
            return NotFound(new { message = $"Estructura con ID '{id}' no fue encontrada." });
        }

        var dto = new StructureDto(
            structure.Id,
            structure.CampusId,
            structure.CategoryId,
            structure.Name,
            structure.Coordinates.Latitude,
            structure.Coordinates.Longitude
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<StructureDto>> Create([FromBody] CreateStructureDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre de la estructura es obligatorio." });
        }

        var campusExists = await _context.Campuses.AnyAsync(c => c.Id == request.CampusId, cancellationToken);
        if (!campusExists)
        {
            return BadRequest(new { message = $"El campus con ID '{request.CampusId}' no existe." });
        }

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return BadRequest(new { message = $"La categoría con ID '{request.CategoryId}' no existe." });
        }

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        var structure = new Structure(request.Name, coordinates, request.CampusId, request.CategoryId);

        _context.Structures.Add(structure);
        await _context.SaveChangesAsync(cancellationToken);

        var responseDto = new StructureDto(
            structure.Id,
            structure.CampusId,
            structure.CategoryId,
            structure.Name,
            structure.Coordinates.Latitude,
            structure.Coordinates.Longitude
        );

        return CreatedAtAction(nameof(GetById), new { id = structure.Id }, responseDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStructureDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre de la estructura es obligatorio." });
        }

        var structure = await _context.Structures.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (structure is null)
        {
            return NotFound(new { message = $"Estructura con ID '{id}' no fue encontrada." });
        }

        var campusExists = await _context.Campuses.AnyAsync(c => c.Id == request.CampusId, cancellationToken);
        if (!campusExists)
        {
            return BadRequest(new { message = $"El campus con ID '{request.CampusId}' no existe." });
        }

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return BadRequest(new { message = $"La categoría con ID '{request.CategoryId}' no existe." });
        }

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        structure.Update(request.Name, coordinates, request.CampusId, request.CategoryId);

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var structure = await _context.Structures.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (structure is null)
        {
            return NotFound(new { message = $"Estructura con ID '{id}' no fue encontrada." });
        }

        _context.Structures.Remove(structure);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
