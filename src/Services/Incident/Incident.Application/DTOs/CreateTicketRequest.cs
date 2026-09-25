using System;

namespace Incident.Application.DTOs;

public record CreateTicketRequest(
    Guid IdUsuario,
    Guid IdStructure,
    int TicketType,
    bool IsActive,
    DateTime DateReport);
