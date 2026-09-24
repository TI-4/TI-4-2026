using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Incident.Domain.Entities;

public class LostObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ObjectId { get; init; }

    [BsonElement("title")]
    public required string title { get; init; }

    [BsonElement("description")]
    public required string description { get; init; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public required Objectenum status { get; init; }

    [BsonElement("photo_url")]
    public string? photo_url { get; init; }
}
