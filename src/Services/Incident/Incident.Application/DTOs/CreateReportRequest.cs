using System;

namespace Incident.Application.DTOs;

public record CreateReportRequest(
    Guid UserRefId,
    Guid StructureRefId,
    int TicketType,
    bool IsActive,
    DateTime ReportedAt);
