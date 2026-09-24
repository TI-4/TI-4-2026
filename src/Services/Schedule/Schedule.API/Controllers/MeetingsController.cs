using Microsoft.AspNetCore.Mvc;
using Schedule.Application.DTOs;
using Schedule.Application.UseCases;

namespace Schedule.API.Controllers;

[ApiController]
[Route("api/schedule/meetings")]
public class MeetingsController : ControllerBase
{
    private readonly MeetingHandler _handler;

    public MeetingsController(MeetingHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MeetingDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _handler.GetByIdAsync(id, cancellationToken);

        if (dto is null)
        {
            return NotFound(new { message = $"Meeting '{id}' was not found." });
        }

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeetingDto>>> GetByTeacher(
        [FromQuery] Guid teacherId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest(new { message = "The 'teacherId' query parameter is required." });
        }

        var (dtos, error) = await _handler.GetByTeacherAsync(teacherId, from, to, cancellationToken);

        if (error is not null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<MeetingDto>> Create(
        [FromBody] CreateMeetingRequest request,
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
