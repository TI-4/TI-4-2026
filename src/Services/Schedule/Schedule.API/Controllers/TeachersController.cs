using Microsoft.AspNetCore.Mvc;
using Schedule.Application.UseCases;

namespace Schedule.API.Controllers;

[ApiController]
[Route("api/schedule/teachers")]
public class TeachersController(GetTeacherWorkloadUseCase getTeacherWorkload) : ControllerBase
{
    [HttpGet("{teacherId:guid}/workload")]
    public async Task<IActionResult> GetWorkload(
        Guid teacherId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest("The 'teacherId' route parameter is required.");
        }

        if (to <= from)
        {
            return BadRequest("The 'to' parameter must be later than 'from'.");
        }

        var workload = await getTeacherWorkload.ExecuteAsync(teacherId, from, to, cancellationToken);

        return Ok(workload);
    }
}
