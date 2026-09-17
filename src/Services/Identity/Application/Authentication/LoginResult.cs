namespace IdentityService.Application.Authentication;

public sealed record LoginResult(string UserId, string Email, string? Token = null);