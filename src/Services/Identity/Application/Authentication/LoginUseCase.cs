namespace IdentityService.Application.Authentication;

public sealed class LoginUseCase(
    IUserAuthenticator userAuthenticator,
    IJwtTokenGenerator jwtTokenGenerator)
{
    public Task<LoginResult?> ExecuteAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        return ExecuteLoginAsync(request, cancellationToken);
    }

    private async Task<LoginResult?> ExecuteLoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await userAuthenticator.AuthenticateAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        var token = await jwtTokenGenerator.GenerateAsync(user, cancellationToken);
        return user with { Token = token };
    }
}