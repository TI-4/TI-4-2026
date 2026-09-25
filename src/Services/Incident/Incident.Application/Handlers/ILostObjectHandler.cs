using ErrorOr;
using Incident.Application.DTOs;
using System.Threading.Tasks;

namespace Incident.Application.Handlers;

public interface ILostObjectHandler
{
    Task<ErrorOr<string>> CreateObjectAsync(CreateLostObjectRequest request);
}

