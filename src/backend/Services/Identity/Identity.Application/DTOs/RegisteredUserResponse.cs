namespace Identity.Application.DTOs;

public sealed record RegisteredUserResponse(
    string UserId,
    string Name,
    string Email,
    string? Role = null);
