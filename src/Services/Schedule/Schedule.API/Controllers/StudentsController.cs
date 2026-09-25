using Microsoft.AspNetCore.Mvc;
using Schedule.Application.DTOs;
using Schedule.Application.UseCases;

namespace Schedule.API.Controllers;

[ApiController]
[Route("api/schedule/students")]
public class StudentsController : ControllerBase
{
    private readonly StudentHandler _handler;

    public StudentsController(StudentHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{studentId:guid}/meetings")]
    public async Task<ActionResult<StudentMeetingsDto>> GetMeetings(
        Guid studentId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        var (dto, error) = await _handler.GetMeetingsAsync(studentId, from, to, cancellationToken);

        if (error is not null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(dto);
    }
}

