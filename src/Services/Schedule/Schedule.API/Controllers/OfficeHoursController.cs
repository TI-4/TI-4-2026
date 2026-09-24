using Microsoft.AspNetCore.Mvc;
using Schedule.Application.DTOs;
using Schedule.Application.UseCases;

namespace Schedule.API.Controllers;

[ApiController]
[Route("api/schedule/office-hours")]
public class OfficeHoursController : ControllerBase
{
    private readonly OfficeHourHandler _handler;

    public OfficeHoursController(OfficeHourHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeHourDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _handler.GetByIdAsync(id, cancellationToken);

        if (dto is null)
        {
            return NotFound(new { message = $"Office hour '{id}' was not found." });
        }

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OfficeHourDto>>> GetByTeacher(
        [FromQuery] Guid teacherId,
        CancellationToken cancellationToken)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest(new { message = "The 'teacherId' query parameter is required." });
        }

        var dtos = await _handler.GetByTeacherAsync(teacherId, cancellationToken);

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<OfficeHourDto>> Create(
        [FromBody] CreateOfficeHourRequest request,
        CancellationToken cancellationToken)
    {
        var (dto, error) = await _handler.CreateAsync(request, cancellationToken);

        if (error is not null)
        {
            return BadRequest(new { message = error });
        }

        return CreatedAtAction(nameof(GetById), new { id = dto!.Id }, dto);
    }
}
