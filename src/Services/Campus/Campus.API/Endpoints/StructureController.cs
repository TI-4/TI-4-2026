using Microsoft.AspNetCore.Mvc;
using Campus.Application.DTOs;
using Campus.Application.UseCases;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/structures")]
public class StructureController : ControllerBase
{
    private readonly StructureHandler _handler;

    public StructureController(StructureHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StructureDto>>> GetAll([FromQuery] Guid? campusId, [FromQuery] Guid? categoryId, CancellationToken cancellationToken)
    {
        var structures = await _handler.GetAllAsync(campusId, categoryId, cancellationToken);
        return Ok(structures);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StructureDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _handler.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound(new { message = $"Estructura con ID '{id}' no fue encontrada." });
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<StructureDto>> Create([FromBody] CreateStructureDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest(new { message = "El nombre es obligatorio." });
        
        var (dto, error) = await _handler.CreateAsync(request, cancellationToken);
        if (error != null) return BadRequest(new { message = error });
        
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStructureDto request, CancellationToken cancellationToken)
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
        if (!success) return NotFound(new { message = $"Estructura con ID '{id}' no fue encontrada." });

        return NoContent();
    }
}
