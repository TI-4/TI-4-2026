namespace Incident.Application.DTOs;

public record CreateTicketRequest(
    string ReporterRefId,
    string Details,
    string Location);
