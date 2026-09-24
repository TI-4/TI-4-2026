using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Incident.Domain.Entities;

public class Complain
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public required Guid UserId { get; init; }

    [BsonElement("structure_id")]
    [BsonRepresentation(BsonType.String)]
    public required Guid StructureId { get; init; }

    [BsonElement("title")]
    public required string Title { get; init; }

    [BsonElement("description")]
    public required string Description { get; init; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public required Complainenum Status { get; init; }

    [BsonElement("date")]
    [BsonRepresentation(BsonType.DateTime)]
    public required DateTime Date { get; init; }

    [BsonElement("location")]
    public required GeoPoint Location { get; init; }
}
