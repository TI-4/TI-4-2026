namespace IdentityService.Application.Authentication;

public interface IUserRegistrar
{
    Task<RegisteredUserResponse?> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default);
}
