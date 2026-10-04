using Identity.Application.DTOs;
namespace Identity.Application.Interfaces;

public interface IUserQuery
{
    Task<RegisteredUserResponse?> GetByIdAsync(
        string userId,
        CancellationToken cancellationToken = default);
}
