using Incident.Application.Protos;
using Incident.Domain.Entities;

namespace Incident.Application.Mappings;

public static class TicketMapper
{
    public static Ticket ToTicket(CreateTicketRequest request) => new()
    {
        IdUsuario = Guid.Parse(request.UserId),
        IdStructure = Guid.Parse(request.StructureId),
        TicketType = MapTicketType(request.TicketType),
        IsActive = request.IsActive,
        DateReport = request.DateReport!.ToDateTime(),
        Location = MapLocation(request.Location),
        ObjectId = request.HasObjectId ? request.ObjectId : null,
        ComplaintDetails = MapComplaintDetails(request.ComplaintDetails),
    };

    private static Tickets MapTicketType(TicketTypeProto value) => value switch
    {
        TicketTypeProto.Claim => Tickets.Claim,
        TicketTypeProto.Found => Tickets.Found,
        TicketTypeProto.Match => Tickets.Match,
        TicketTypeProto.Pickup => Tickets.Pickup,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "TicketType sin mapeo.")
    };

    private static GeoPoint? MapLocation(GeoPointMessage? location) =>
        location is null ? null : new GeoPoint(location.Longitude, location.Latitude);

    private static ComplaintDetails? MapComplaintDetails(ComplaintDetailsMessage? details) =>
        details is null ? null : new ComplaintDetails
        {
            Title = details.Title,
            Description = details.Description,
            Status = MapComplaintStatus(details.Status),
        };

    private static Complainenum MapComplaintStatus(ComplaintStatusProto value) => value switch
    {
        ComplaintStatusProto.Pending => Complainenum.Pending,
        ComplaintStatusProto.InProcess => Complainenum.In_Process,
        ComplaintStatusProto.Resolved => Complainenum.Resolved,
        ComplaintStatusProto.Dismissed => Complainenum.Dismissed,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "ComplaintStatus sin mapeo.")
    };
}
