using ErrorOr;

namespace IdentityService.Application.Authentication;

public sealed class LoginUseCase(
    IUserAuthenticator userAuthenticator,
    IJwtTokenGenerator jwtTokenGenerator)
{
    public async Task<ErrorOr<LoginResult>> ExecuteAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await userAuthenticator.AuthenticateAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (user is null)
        {
            return Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "Invalid email or password.");
        }

        var token = await jwtTokenGenerator.GenerateAsync(user, cancellationToken);
        return user with { Token = token };
    }
}