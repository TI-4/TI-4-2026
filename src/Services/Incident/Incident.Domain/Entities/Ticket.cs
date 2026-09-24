using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Incident.Domain.Entities;

public class Ticket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public required Guid IdUsuario { get; init; }

    [BsonElement("structure_id")]
    public required Guid IdStructure { get; init; }

    [BsonElement("ticket_type")]
    [BsonRepresentation(BsonType.String)]
    public required Tickets TicketType { get; set; }

    [BsonElement("is_active")]
    [BsonRepresentation(BsonType.Boolean)]
    public required bool IsActive { get; set; }

    [BsonElement("date_ticket")]
    [BsonRepresentation(BsonType.DateTime)]
    public required DateTime DateReport { get; init; }

    [BsonElement("location")]
    public GeoPoint? Location { get; set; }

    // Optional Reference to the independent Object collection
    [BsonElement("object_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ObjectId { get; init; }

    // Optional Embedded Complaint details
    [BsonElement("complaint_details")]
    public ComplaintDetails? ComplaintDetails { get; init; }
}
