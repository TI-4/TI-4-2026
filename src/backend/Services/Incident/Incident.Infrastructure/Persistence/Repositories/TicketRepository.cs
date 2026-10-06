using Incident.Domain.Entities;
using Incident.Domain.Interfaces;
using Incident.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Incident.Infrastructure.Persistence.Repositories;

public class TicketRepository : MongoRepository<Ticket>, ITicketRepository
{
    public TicketRepository(IMongoDbContext context)
        : base(context, "tickets")
    { }
    public async Task<bool> UpdateStatusAsync(Ticket ticket, int status)
    {
        ticket.TicketType = (Tickets)status;

        var filter = Builders<Ticket>.Filter.Eq(x => x.Id, ticket.Id);
        var result = await _collection.ReplaceOneAsync(filter, ticket);

        return result.ModifiedCount > 0;
    }
}
