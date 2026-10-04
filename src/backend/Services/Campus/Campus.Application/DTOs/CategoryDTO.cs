using System;

namespace Campus.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Icon,
    string Description
);

public record CreateCategoryDto(
    string Name,
    string Icon,
    string Description
);

public record UpdateCategoryDto(
    string Name,
    string Icon,
    string Description
);
