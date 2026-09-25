using Incident.Application.DTOs;
using Incident.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Incident.API.Controllers;

[ApiController]
[Route("api/incident/object")]
public class ObjectsController : ControllerBase
{
    private readonly ObjectHandler _objectHandler;

    public ObjectsController(ObjectHandler objectHandler){
        _objectHandler = objectHandler;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateObjectRequest request)
    {
        var result = await _objectHandler.CreateObjectAsync(request);

        return result.Match(
            createdObject => CreatedAtAction(
                nameof(GetById),
                new { id = createdObject.ObjectId },
                createdObject
            ),
            errors => Problem(statusCode: StatusCodes.Status400BadRequest, title: errors.First().Description)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(string id)
    {
        return Ok();
    }
}
