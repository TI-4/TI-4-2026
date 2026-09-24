using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Incident.Domain.Entities;

public class Ticket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ObjectID { get; init; }

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public required Guid IdUsuario { get; init; }

    [BsonElement("structure_id")]
    public required Guid IdStructure { get; init; }

    [BsonElement("ticket_type")]
    [BsonRepresentation(BsonType.String)]
    public required Tickets ticket_type { get; set; }

    [BsonElement("is_active")]
    [BsonRepresentation(BsonType.Boolean)]
    public required bool IsActive { get; set; }

    [BsonElement("date_ticket")]
    [BsonRepresentation(BsonType.DateTime)]
    public required DateTime DateReport { get; init; }

    [BsonElement("location")]
    public GeoPoint? location { get; set; }
}
