using System;
using Incident.Domain.Entities;
using MongoDB.Bson;

namespace Incident.Application.DTOs;

public record CreateReportRequest(
        Guid UserRefId,
        Guid StructureRefId,
        int TicketType,
        bool IsActive,
        DateTime ReportedAt,
        string? LostObjectId = null,
        ComplaintDetails? Complaint = null);

public record UpdateStatusReport(
    int Status,
    int? ComplainStatus = null
);
