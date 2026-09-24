using System;

namespace Campus.Application.DTOs;

public record BuildingDto(
    Guid Id,
    Guid CampusId,
    string Name,
    int FloorsCount,
    double Latitude,
    double Longitude
);

public record CreateBuildingDto(
    Guid CampusId,
    string Name,
    int FloorsCount,
    double Latitude,
    double Longitude
);

public record UpdateBuildingDto(
    Guid CampusId,
    string Name,
    int FloorsCount,
    double Latitude,
    double Longitude
);
