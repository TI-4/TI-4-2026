
using Incident.Domain.Entities;

namespace Incident.Application.DTOs;

public record CreateObjectRequest(
    string Title,
    string Description,
    int Status,
    string PhotoUrl
);
