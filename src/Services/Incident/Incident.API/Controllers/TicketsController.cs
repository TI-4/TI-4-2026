using Incident.Application.Protos;
using Incident.Application.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest? request)
    {
        var id = await _ticketService.CreateTicketAsync(request);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok();
    }
}