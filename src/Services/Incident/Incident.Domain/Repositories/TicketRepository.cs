using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using MongoDB.Driver;

namespace Incident.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Ticket> _tickets;

    public TicketRepository(IMongoDatabase database)
    {
        _tickets = database.GetCollection<Ticket>("tickets");
    }

    public async Task<Ticket?> GetByIdAsync(string id) =>
        await _tickets.Find(t => t.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Ticket>> GetAllAsync() =>
        await _tickets.Find(_ => true).ToListAsync();

    public async Task CreateAsync(Ticket entity) =>
        await _tickets.InsertOneAsync(entity);

    public async Task UpdateAsync(string id, Ticket entity) =>
        await _tickets.ReplaceOneAsync(t => t.Id == id, entity);

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _tickets.DeleteOneAsync(t => t.Id == id);
        return result.DeletedCount > 0;
    }
}
