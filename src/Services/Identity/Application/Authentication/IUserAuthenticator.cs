namespace IdentityService.Application.Authentication;

public interface IUserAuthenticator
{
    Task<LoginResult?> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}