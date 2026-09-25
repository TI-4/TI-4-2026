using Incident.Application.DTOs;
using Incident.Application.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Incident.API.Controllers;

[ApiController]
[Route("api/incident/lost-objects")]
public class LostObjectsController : ControllerBase
{
    private readonly ILostObjectHandler _lostObjectHandler;

    public LostObjectsController(ILostObjectHandler lostObjectHandler)
    {
        _lostObjectHandler = lostObjectHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLostObjectRequest request)
    {
        var result = await _lostObjectHandler.CreateObjectAsync(request);

        return result.Match(
            id => CreatedAtAction(nameof(GetById), new { id }, new { id }),
            errors => Problem(statusCode: StatusCodes.Status400BadRequest, title: errors.First().Description)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok();
    }
}

