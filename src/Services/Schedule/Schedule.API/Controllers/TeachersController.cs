using Microsoft.AspNetCore.Mvc;
using Schedule.Application.DTOs;
using Schedule.Application.UseCases;

namespace Schedule.API.Controllers;

[ApiController]
[Route("api/schedule/teachers")]
public class TeachersController : ControllerBase
{
    private readonly TeacherHandler _handler;

    public TeachersController(TeacherHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{teacherId:guid}/workload")]
    public async Task<ActionResult<TeacherWorkloadDto>> GetWorkload(
        Guid teacherId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        var (dto, error) = await _handler.GetWorkloadAsync(teacherId, from, to, cancellationToken);

        if (error is not null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(dto);
    }

    [HttpGet("{teacherId:guid}/availability")]
    public async Task<ActionResult<TeacherAvailabilityDto>> GetAvailability(
        Guid teacherId,
        CancellationToken cancellationToken)
    {
        var dto = await _handler.GetAvailabilityAsync(teacherId, cancellationToken);

        return Ok(dto);
    }
}
