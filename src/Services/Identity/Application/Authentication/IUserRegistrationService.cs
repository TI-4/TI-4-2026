namespace IdentityService.Application.Authentication;

public interface IUserRegistrationService
{
    Task<RegisteredUserResponse?> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default);
}

