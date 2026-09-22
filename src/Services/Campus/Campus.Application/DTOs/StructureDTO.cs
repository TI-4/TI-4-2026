using System;

namespace Campus.Application.DTOs;

public record StructureDto(
    Guid Id,
    Guid CampusId,
    Guid CategoryId,
    string Name,
    double Latitude,
    double Longitude
);

public record CreateStructureDto(
    Guid CampusId,
    Guid CategoryId,
    string Name,
    double Latitude,
    double Longitude
);

public record UpdateStructureDto(
    Guid CampusId,
    Guid CategoryId,
    string Name,
    double Latitude,
    double Longitude
);
