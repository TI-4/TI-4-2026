using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Campus.Domain.Entities;
using Campus.Application.DTOs;
using Campus.Infraestructure.Persistence;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly CampusDbContext _context;

    public RoomController(CampusDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll(
        [FromQuery] Guid? buildingId,
        [FromQuery] Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var query = _context.Rooms.AsNoTracking();

        if (buildingId.HasValue)
        {
            query = query.Where(r => r.BuildingId == buildingId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(r => r.CategoryId == categoryId.Value);
        }

        var rooms = await query
            .Select(r => new RoomDto(
                r.Id,
                r.BuildingId,
                r.CategoryId,
                r.Name,
                r.Floor,
                r.Number
            ))
            .ToListAsync(cancellationToken);

        return Ok(rooms);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (room is null)
        {
            return NotFound(new { message = $"Sala con ID '{id}' no fue encontrada." });
        }

        var dto = new RoomDto(
            room.Id,
            room.BuildingId,
            room.CategoryId,
            room.Name,
            room.Floor,
            room.Number
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create([FromBody] CreateRoomDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre de la sala es obligatorio." });
        }

        var buildingExists = await _context.Buildings.AnyAsync(b => b.Id == request.BuildingId, cancellationToken);
        if (!buildingExists)
        {
            return BadRequest(new { message = $"El edificio con ID '{request.BuildingId}' no existe." });
        }

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return BadRequest(new { message = $"La categoría con ID '{request.CategoryId}' no existe." });
        }

        var room = new Room(request.Name, request.Floor, request.Number, request.BuildingId, request.CategoryId);

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);

        var responseDto = new RoomDto(
            room.Id,
            room.BuildingId,
            room.CategoryId,
            room.Name,
            room.Floor,
            room.Number
        );

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, responseDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre de la sala es obligatorio." });
        }

        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (room is null)
        {
            return NotFound(new { message = $"Sala con ID '{id}' no fue encontrada." });
        }

        var buildingExists = await _context.Buildings.AnyAsync(b => b.Id == request.BuildingId, cancellationToken);
        if (!buildingExists)
        {
            return BadRequest(new { message = $"El edificio con ID '{request.BuildingId}' no existe." });
        }

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return BadRequest(new { message = $"La categoría con ID '{request.CategoryId}' no existe." });
        }

        room.Update(request.Name, request.Floor, request.Number, request.BuildingId, request.CategoryId);

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (room is null)
        {
            return NotFound(new { message = $"Sala con ID '{id}' no fue encontrada." });
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
