using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Incident.Domain.Entities;

public class ComplaintDetails
{
    [BsonElement("title")]
    public required string Title { get; init; }

    [BsonElement("description")]
    public required string Description { get; init; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public required Complainenum Status { get; init; }
}
