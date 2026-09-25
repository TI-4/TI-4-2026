using Incident.Domain.Entities;
using MongoDB.Bson;
using System;

namespace Incident.Application.DTOs;

public record ReportResponse(
    string Id,
    Guid UserRefId,
    Guid? StructureRefId,
    Tickets TicketType,
    bool IsActive,
    DateTime ReportedAt,
    ComplaintDetails? ComplaintDetails = null,
    string? LostObjectId = null);
