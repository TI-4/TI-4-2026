using Microsoft.AspNetCore.Mvc;
using Schedule.Application.DTOs;
using Schedule.Domain.Entities;
using Schedule.Domain.Interfaces;

namespace Schedule.API.Controllers;

[ApiController]
[Route("api/schedule/office-hours")]
public class OfficeHoursController(IOfficeHourRepository repository) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var officeHour = await repository.GetByIdAsync(id, cancellationToken);

        if (officeHour is null)
        {
            return NotFound();
        }

        return Ok(OfficeHourDto.FromEntity(officeHour));
    }

    [HttpGet]
    public async Task<IActionResult> GetByTeacher(
        [FromQuery] Guid teacherId,
        CancellationToken cancellationToken)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest("The 'teacherId' query parameter is required.");
        }

        var officeHours = await repository.GetByTeacherAsync(teacherId, cancellationToken);

        return Ok(officeHours.Select(OfficeHourDto.FromEntity));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOfficeHourRequest request,
        CancellationToken cancellationToken)
    {
        OfficeHour officeHour;

        try
        {
            officeHour = new OfficeHour(
                request.TeacherRefId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        await repository.AddAsync(officeHour, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = officeHour.Id },
            OfficeHourDto.FromEntity(officeHour));
    }
}
