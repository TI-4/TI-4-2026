using Microsoft.AspNetCore.Mvc;

namespace Incident.API.Controllers;

[ApiController]
[Route("api/incident/[controller]")]
public class ComplaintsController : ControllerBase
{
    [HttpPost]
    public async Task Create()
    {
    }

    [HttpGet("{id}")]
    public async Task GetById(string id)
    {
    }
}