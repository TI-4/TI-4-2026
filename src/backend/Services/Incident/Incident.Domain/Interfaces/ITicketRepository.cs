using Incident.Domain.Entities;
namespace Incident.Domain.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<bool> UpdateStatusAsync(Ticket ticket, int status);
    Task<bool> UpdateStatusComplainAsync(Ticket ticket, int? status);
}
