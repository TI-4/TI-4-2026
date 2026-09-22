using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Campus.Domain.Entities;
using Campus.Domain.ValueObjects;
using Campus.Application.DTOs;
using Campus.Infraestructure.Persistence;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/buildings")]
public class BuildingController : ControllerBase
{
    private readonly CampusDbContext _context;

    public BuildingController(CampusDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAll(
        [FromQuery] Guid? campusId,
        CancellationToken cancellationToken)
    {
        var query = _context.Buildings.AsNoTracking();

        if (campusId.HasValue)
        {
            query = query.Where(b => b.CampusId == campusId.Value);
        }

        var buildings = await query
            .Select(b => new BuildingDto(
                b.Id,
                b.CampusId,
                b.Name,
                b.FloorsCount,
                b.Coordinates.Latitude,
                b.Coordinates.Longitude
            ))
            .ToListAsync(cancellationToken);

        return Ok(buildings);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BuildingDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var building = await _context.Buildings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (building is null)
        {
            return NotFound(new { message = $"Edificio con ID '{id}' no fue encontrado." });
        }

        var dto = new BuildingDto(
            building.Id,
            building.CampusId,
            building.Name,
            building.FloorsCount,
            building.Coordinates.Latitude,
            building.Coordinates.Longitude
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<BuildingDto>> Create([FromBody] CreateBuildingDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre del edificio es obligatorio." });
        }

        var campusExists = await _context.Campuses.AnyAsync(c => c.Id == request.CampusId, cancellationToken);
        if (!campusExists)
        {
            return BadRequest(new { message = $"El campus con ID '{request.CampusId}' no existe." });
        }

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        var building = new Building(request.Name, request.FloorsCount, coordinates, request.CampusId);

        _context.Buildings.Add(building);
        await _context.SaveChangesAsync(cancellationToken);

        var responseDto = new BuildingDto(
            building.Id,
            building.CampusId,
            building.Name,
            building.FloorsCount,
            building.Coordinates.Latitude,
            building.Coordinates.Longitude
        );

        return CreatedAtAction(nameof(GetById), new { id = building.Id }, responseDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBuildingDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre del edificio es obligatorio." });
        }

        var building = await _context.Buildings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (building is null)
        {
            return NotFound(new { message = $"Edificio con ID '{id}' no fue encontrado." });
        }

        var campusExists = await _context.Campuses.AnyAsync(c => c.Id == request.CampusId, cancellationToken);
        if (!campusExists)
        {
            return BadRequest(new { message = $"El campus con ID '{request.CampusId}' no existe." });
        }

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        building.Update(request.Name, request.FloorsCount, coordinates, request.CampusId);

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var building = await _context.Buildings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (building is null)
        {
            return NotFound(new { message = $"Edificio con ID '{id}' no fue encontrado." });
        }

        _context.Buildings.Remove(building);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
