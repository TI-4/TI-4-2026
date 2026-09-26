using Incident.Domain.Entities;
namespace Incident.Domain.Repositories;
public interface ILostObjectRepository : IRepository<LostObject>{
    Task<List<LostObject>> FilterStatusAsync(string status);
    Task<bool> UpdateStatusAsync(LostObject lostObject, int status);
}
