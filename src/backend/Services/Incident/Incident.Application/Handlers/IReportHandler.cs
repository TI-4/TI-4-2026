using ErrorOr;
using Incident.Application.DTOs;
using System.Threading.Tasks;

namespace Incident.Application.Handlers;

public interface IReportHandler
{
    Task<ErrorOr<string>> CreateReportAsync(CreateReportRequest request);
    Task<ErrorOr<TicketResponse>> GetByIdAsync(string id);
}

