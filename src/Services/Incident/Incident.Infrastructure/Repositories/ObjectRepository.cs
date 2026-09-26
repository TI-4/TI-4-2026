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

    public override async Task CreateAsync(LostObject entity)
    {
        await base.CreateAsync(entity);
    }

    public async Task<List<LostObject>> FilterStatusAsync(string status)
    {
        return await _collection.Find(x => x.Status.ToString() == status).ToListAsync();
    }
    public async Task<bool> UpdateStatusAsync(LostObject lostObject, int status)
    {
        lostObject.Status = (Objectenum)status;

        var filter = Builders<LostObject>.Filter.Eq(x => x.ObjectId, lostObject.ObjectId);
        var result = await _collection.ReplaceOneAsync(filter, lostObject);

        return result.ModifiedCount > 0;
    }
}
