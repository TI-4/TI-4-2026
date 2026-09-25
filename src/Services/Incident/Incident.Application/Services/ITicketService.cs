using ErrorOr;
using Incident.Application.DTOs;
using System.Threading.Tasks;

namespace Incident.Application.Services;

public interface ITicketService
{
    Task<ErrorOr<string>> CreateTicketAsync(CreateTicketRequest request);
}
