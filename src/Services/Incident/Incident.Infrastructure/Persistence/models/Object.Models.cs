using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ObjectModels;
public enum Objectenum {
    Pending,
    In_Process,
    Resolved,
    Canceled,
}
public class LostObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String? ObjectId { get; init; }

    [BsonElement("title")]
    public required String title { get; init; }

    [BsonElement("description")]
    public required String description { get; init; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public required Objectenum status { get; init; }

    [BsonElement("photo_url")]
    public String? photo_url { get; init; }




}
