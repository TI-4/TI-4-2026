using ErrorOr;
using Incident.Application.DTOs;
using Incident.Domain.Entities;
using System.Threading.Tasks;

namespace Incident.Application.Handlers;

public interface ILostObjectHandler
{
    Task<ErrorOr<string>> CreateObjectAsync(CreateLostObjectRequest request);
    Task<ErrorOr<LostObjectResponse>> GetByIdAsync(string id);
    Task<ErrorOr<LostObjectList>> FilterStatusAsync(int status);
    Task<ErrorOr<Success>> UpdateStatusAsync(string id, UpdateStatus status);
}

