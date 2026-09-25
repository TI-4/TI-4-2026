using Incident.Domain.Entities;
using MongoDB.Driver;
namespace Incident.Domain.Repositories;
public interface ITicketRepository : IRepository<Ticket>{
    //Es posible añadir mas metodos aqui
}
