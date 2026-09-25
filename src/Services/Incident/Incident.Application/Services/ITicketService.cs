using Incident.Application.Protos;

namespace Incident.Application.Services;

public interface ITicketService
{
    Task<string> CreateTicketAsync(CreateTicketRequest? request);
}
