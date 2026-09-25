using Microsoft.AspNetCore.Mvc;
using Campus.Application.DTOs;
using Campus.Application.UseCases;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/buildings")]
public class BuildingController : ControllerBase
{
    private readonly BuildingHandler _handler;

    public BuildingController(BuildingHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAll([FromQuery] Guid? campusId, CancellationToken cancellationToken)
    {
        var buildings = await _handler.GetAllAsync(campusId, cancellationToken);
        return Ok(buildings);
    }

    [HttpGet("locations")]
    public async Task<ActionResult<IEnumerable<BuildingLocationDto>>> GetLocations([FromQuery] Guid? campusId, CancellationToken cancellationToken)
    {
        var buildings = await _handler.GetAllLocationsAsync(campusId, cancellationToken);
        return Ok(buildings);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BuildingDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _handler.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound(new { message = $"Edificio con ID '{id}' no fue encontrado." });
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<BuildingDto>> Create([FromBody] CreateBuildingDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest(new { message = "El nombre es obligatorio." });
        
        var (dto, error) = await _handler.CreateAsync(request, cancellationToken);
        if (error != null) return BadRequest(new { message = error });
        
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBuildingDto request, CancellationToken cancellationToken)
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
        if (!success) return NotFound(new { message = $"Edificio con ID '{id}' no fue encontrado." });

        return NoContent();
    }
}
