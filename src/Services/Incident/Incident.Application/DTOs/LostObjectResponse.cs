
using Incident.Domain.Entities;

namespace Incident.Application.DTOs;

public record LostObjectResponse(
    string Title,
    string Description,
    string Status,
    string PhotoUrl,
    Guid StructureId // -> FindName
);
