
using Incident.Domain.Entities;

namespace Incident.Application.DTOs;

public record CreateLostObjectRequest(
    string Title,
    string Description,
    int Status,
    string PhotoUrl,
    Guid StructureId
);


public record UpdateStatus(int Status);
