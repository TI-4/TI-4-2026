using System.Reflection.Metadata;
using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using Incident.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Incident.Infrastructure.Repositories;

public class ObjectRepository : MongoRepository<LostObject>, ILostObjectRepository
{
    public ObjectRepository(IMongoDbContext context)
        : base(context, "lost_objects")
    { }

    public override async Task CreateAsync(LostObject entity){
        await base.CreateAsync(entity);
    }
}
