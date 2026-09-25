using Incident.Application.DTOs;
using Incident.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Incident.API.Controllers;

[ApiController]
[Route("api/incident/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest request)
    {
        var result = await _ticketService.CreateTicketAsync(request);

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
