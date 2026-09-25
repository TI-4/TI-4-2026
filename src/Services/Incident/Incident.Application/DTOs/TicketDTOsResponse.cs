using Incident.Domain.Entities;
using MongoDB.Bson;

namespace Incident.Application.DTOs;
public record TicketResponse(
    string id,
    Guid UserRefId,
    Guid? StructureRefId,
    Tickets TicketType,
    bool IsActive,
    DateTime ReportedAt,
    ComplaintDetails? complaintDetails = null,
    string? ObjectLost = null);
