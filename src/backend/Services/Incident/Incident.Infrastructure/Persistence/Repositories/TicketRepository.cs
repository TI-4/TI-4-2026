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
}
