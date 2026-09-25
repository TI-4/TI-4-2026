namespace IdentityService.Application.Authentication;

public interface IUserQuery
{
    Task<RegisteredUserResponse?> GetByIdAsync(
        string userId,
        CancellationToken cancellationToken = default);
}
