using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Incident.Domain.Entities;

public class ComplaintDetails
{
    [BsonElement("title")]
    public required string Title { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public required Complainenum Status { get; set; }
}
