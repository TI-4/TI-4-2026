using Incident.Application.Mappings;
using Incident.Application.Protos;
using Incident.Application.Validation;
using Incident.Domain.Repositories;

namespace Incident.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<string> CreateTicketAsync(CreateTicketRequest? request)
    {
        var validated = TicketRequestValidator.Validate(request);

        var ticket = TicketMapper.ToTicket(validated);

        await _ticketRepository.CreateAsync(ticket);

        return ticket.Id!;
    }
}
