using ErrorOr;

namespace IdentityService.Application.Authentication;

public sealed class GetUserByIdUseCase(IUserQuery userQuery)
{
    public async Task<ErrorOr<RegisteredUserResponse>> ExecuteAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Validation(
                code: "Identity.GetUser.InvalidRequest",
                description: "User id is required.");
        }

        var user = await userQuery.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Error.NotFound(
                code: "Identity.GetUser.NotFound",
                description: "User was not found.");
        }

        return user;
    }
}
