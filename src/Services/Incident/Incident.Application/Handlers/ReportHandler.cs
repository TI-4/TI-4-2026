using ErrorOr;
using Incident.Application.DTOs;
using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using System.Threading.Tasks;
using System;

namespace Incident.Application.Handlers;

public class ReportHandler : IReportHandler
{
    private readonly ITicketRepository _ticketRepository;

    public ReportHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<ErrorOr<string>> CreateReportAsync(CreateReportRequest request)
    {
        if (request.UserRefId == Guid.Empty)
        {
            return Error.Validation("Report.UserRefId", "User ID is required.");
        }

        if (request.StructureRefId == Guid.Empty)
        {
            return Error.Validation("Report.StructureRefId", "Structure ID is required.");
        }

        var ticket = new Ticket
        {
            UserRefId = request.UserRefId,
            StructureRefId = request.StructureRefId,
            TicketType = (Tickets)request.TicketType,
            IsActive = request.IsActive,
            ReportedAt = request.ReportedAt
        };

        await _ticketRepository.CreateAsync(ticket);

        return ticket.Id ?? string.Empty;
    }

    public async Task<ErrorOr<ReportResponse>> GetByIdAsync(string id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket is null)
        {
            return Error.NotFound(
                code: "Report.NotFound",
                description: $"Report with ID '{id}'"
            );
        }

        return new ReportResponse(
            ticket.Id!,
            ticket.UserRefId,
            ticket.StructureRefId,
            (Tickets)ticket.TicketType,
            ticket.IsActive,
            ticket.ReportedAt,
            ticket.ComplaintDetails,
            ticket.LostObjectId
        );
    }
}

