namespace IdentityService.Application.Authentication;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string Password,
    string? Role = null);
