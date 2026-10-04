using System;

namespace Campus.Application.DTOs;

public record CampusDto(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude
);

public record CreateCampusDto(
    string Name,
    string Address,
    double Latitude,
    double Longitude
);

public record UpdateCampusDto(
    string Name,
    string Address,
    double Latitude,
    double Longitude
);
