using System;

namespace Campus.Application.DTOs;

public record RoomDto(
    Guid Id,
    Guid BuildingId,
    Guid CategoryId,
    string Name,
    int Floor,
    string? Number
);

public record CreateRoomDto(
    Guid BuildingId,
    Guid CategoryId,
    string Name,
    int Floor,
    string? Number
);

public record UpdateRoomDto(
    Guid BuildingId,
    Guid CategoryId,
    string Name,
    int Floor,
    string? Number
);
