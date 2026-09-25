using ErrorOr;
using Incident.Application.DTOs;
using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using System.Threading.Tasks;

namespace Incident.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<ErrorOr<string>> CreateTicketAsync(CreateTicketRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Details))
        {
            return Error.Validation("Ticket.Details", "Ticket details are required.");
        }

        if (string.IsNullOrWhiteSpace(request.Location))
        {
            return Error.Validation("Ticket.Location", "Ticket location is required.");
        }

        var ticket = new Ticket
        {
            ReporterRefId = request.ReporterRefId,
            Details = request.Details,
            Location = request.Location,
            Status = "Open"
        };

        await _ticketRepository.CreateAsync(ticket);

        return ticket.Id!;
    }
}
