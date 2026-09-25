using ErrorOr;
using Incident.Application.DTOs;
using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using System.Threading.Tasks;
using System;

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
        if (request.IdUsuario == Guid.Empty)
        {
            return Error.Validation("Ticket.IdUsuario", "User ID is required.");
        }

        if (request.IdStructure == Guid.Empty)
        {
            return Error.Validation("Ticket.IdStructure", "Structure ID is required.");
        }

        var ticket = new Ticket
        {
            IdUsuario = request.IdUsuario,
            IdStructure = request.IdStructure,
            TicketType = (Tickets)request.TicketType,
            IsActive = request.IsActive,
            DateReport = request.DateReport
        };

        await _ticketRepository.CreateAsync(ticket);

        return ticket.Id ?? string.Empty;
    }
}
