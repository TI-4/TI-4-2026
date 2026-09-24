using Incident.Application.Protos;
using Microsoft.AspNetCore.Mvc;

namespace Incident.API.Controllers;

[ApiController]
[Route("api/incident/[controller]")]
public class TicketsController : ControllerBase
{
    [HttpPost]
    public async Task Create([FromBody] CreateTicketRequest request)
    {
    }

    [HttpGet("{id}")]
    public async Task GetById(string id)
    {
    }
}
