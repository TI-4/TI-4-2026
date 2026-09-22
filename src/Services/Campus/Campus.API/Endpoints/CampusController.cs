using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Campus.Domain.ValueObjects;
using Campus.Application.DTOs;
using Campus.Infraestructure.Persistence;
using CampusEntity = Campus.Domain.Entities.Campus;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/campuses")]
public class CampusController : ControllerBase
{
    private readonly CampusDbContext _context;

    public CampusController(CampusDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CampusDto>>> GetAll(CancellationToken cancellationToken)
    {
        var campuses = await _context.Campuses
            .AsNoTracking()
            .Select(c => new CampusDto(
                c.Id,
                c.Name,
                c.Address,
                c.Coordinates.Latitude,
                c.Coordinates.Longitude
            ))
            .ToListAsync(cancellationToken);

        return Ok(campuses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CampusDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var campus = await _context.Campuses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (campus is null)
        {
            return NotFound(new { message = $"Campus con ID '{id}' no fue encontrado." });
        }

        var dto = new CampusDto(
            campus.Id,
            campus.Name,
            campus.Address,
            campus.Coordinates.Latitude,
            campus.Coordinates.Longitude
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CampusDto>> Create([FromBody] CreateCampusDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre del campus es obligatorio." });
        }

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        var campus = new CampusEntity(request.Name, request.Address, coordinates);

        _context.Campuses.Add(campus);
        await _context.SaveChangesAsync(cancellationToken);

        var responseDto = new CampusDto(
            campus.Id,
            campus.Name,
            campus.Address,
            campus.Coordinates.Latitude,
            campus.Coordinates.Longitude
        );

        return CreatedAtAction(nameof(GetById), new { id = campus.Id }, responseDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCampusDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre del campus es obligatorio." });
        }

        var campus = await _context.Campuses.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (campus is null)
        {
            return NotFound(new { message = $"Campus con ID '{id}' no fue encontrado." });
        }

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        campus.Update(request.Name, request.Address, coordinates);

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var campus = await _context.Campuses.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (campus is null)
        {
            return NotFound(new { message = $"Campus con ID '{id}' no fue encontrado." });
        }

        _context.Campuses.Remove(campus);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
