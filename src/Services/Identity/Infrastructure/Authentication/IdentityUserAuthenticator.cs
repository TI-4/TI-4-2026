using IdentityService.Application.Authentication;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Authentication;

public sealed class IdentityUserAuthenticator(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator) : IUserAuthenticator
{
    public async Task<LoginResult?> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }

        var resultWithoutToken = new LoginResult(user.Id, user.Email ?? email);
        var token = await jwtTokenGenerator.GenerateAsync(resultWithoutToken, cancellationToken);

        return resultWithoutToken with { Token = token };
    }
}