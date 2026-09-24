namespace IdentityService.Application.Authentication;

public interface IJwtTokenGenerator
{
    Task<string> GenerateAsync(LoginResult user, CancellationToken cancellationToken = default);
}