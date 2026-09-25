using Microsoft.AspNetCore.Mvc;
using Campus.Application.DTOs;
using Campus.Application.UseCases;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly RoomHandler _handler;

    public RoomController(RoomHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll([FromQuery] Guid? buildingId, [FromQuery] Guid? categoryId, CancellationToken cancellationToken)
    {
        var rooms = await _handler.GetAllAsync(buildingId, categoryId, cancellationToken);
        return Ok(rooms);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> Search([FromQuery] string term, CancellationToken cancellationToken)
    {
        var rooms = await _handler.SearchRoomsAsync(term ?? string.Empty, cancellationToken);
        return Ok(rooms);
    }

    [HttpGet("/api/buildings/{buildingId:guid}/rooms")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetByBuilding(Guid buildingId, CancellationToken cancellationToken)
    {
        var rooms = await _handler.GetRoomsByBuildingAsync(buildingId, cancellationToken);
        return Ok(rooms);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _handler.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound(new { message = $"Sala con ID '{id}' no fue encontrada." });
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create([FromBody] CreateRoomDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest(new { message = "El nombre es obligatorio." });
        
        var (dto, error) = await _handler.CreateAsync(request, cancellationToken);
        if (error != null) return BadRequest(new { message = error });
        
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest(new { message = "El nombre es obligatorio." });

        var (success, error) = await _handler.UpdateAsync(id, request, cancellationToken);
        if (error != null) return BadRequest(new { message = error });
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var success = await _handler.DeleteAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = $"Sala con ID '{id}' no fue encontrada." });

        return NoContent();
    }
}
