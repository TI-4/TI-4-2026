using Incident.Domain.Repositories;
using Incident.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Incident.Infrastructure.Repositories;

public class MongoRepository<T> : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoDbContext context, string collectionName)
    {
        _collection = context.GetCollection<T>(collectionName);
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _collection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(T entity)
        => await _collection.InsertOneAsync(entity);

    public async Task UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
}