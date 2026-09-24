using Microsoft.AspNetCore.Mvc;
using Schedule.Application.DTOs;
using Schedule.Domain.Entities;
using Schedule.Domain.Interfaces;

namespace Schedule.API.Controllers;

[ApiController]
[Route("api/schedule/meetings")]
public class MeetingsController(IMeetingRepository repository) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var meeting = await repository.GetByIdAsync(id, cancellationToken);

        if (meeting is null)
        {
            return NotFound();
        }

        return Ok(MeetingDto.FromEntity(meeting));
    }

    [HttpGet]
    public async Task<IActionResult> GetByTeacher(
        [FromQuery] Guid teacherId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest("The 'teacherId' query parameter is required.");
        }

        if (to <= from)
        {
            return BadRequest("The 'to' parameter must be later than 'from'.");
        }

        var meetings = await repository.GetByTeacherAsync(teacherId, from, to, cancellationToken);

        return Ok(meetings.Select(MeetingDto.FromEntity));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMeetingRequest request,
        CancellationToken cancellationToken)
    {
        var meeting = new Meeting(
            request.TeacherRefId,
            request.StudentRefId,
            request.StructureRefId,
            request.ScheduledAt);

        await repository.AddAsync(meeting, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = meeting.Id },
            MeetingDto.FromEntity(meeting));
    }
}
