using IdentityService.Application.Authentication;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Authentication;

public sealed class IdentityUserQuery(UserManager<User> userManager) : IUserQuery
{
    public async Task<RegisteredUserResponse?> GetByIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);

        return new RegisteredUserResponse(
            user.Id,
            user.Name,
            user.Email ?? string.Empty,
            roles.FirstOrDefault());
    }
}
