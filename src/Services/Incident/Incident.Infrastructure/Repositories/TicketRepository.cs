using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using Incident.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Incident.Infrastructure.Repositories;

public class TicketRepository : MongoRepository<Ticket>, ITicketRepository
{
    public TicketRepository(IMongoDbContext context)
        : base(context, "tickets")
    { }
}
