namespace IdentityService.Application.Authentication;

public sealed class LoginUseCase(IUserAuthenticator userAuthenticator)
{
    public Task<LoginResult?> ExecuteAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        return userAuthenticator.AuthenticateAsync(
            request.Email,
            request.Password,
            cancellationToken);
    }
}