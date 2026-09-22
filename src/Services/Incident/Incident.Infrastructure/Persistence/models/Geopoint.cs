namespace Incident.Infrastructure.Persistence.Models;

using MongoDB.Bson.Serialization.Attributes;

public class GeoPoint
{
    [BsonElement("type")]
    public string Type { get; set; } = "Point";

    [BsonElement("coordinates")]
    public double[] Coordinates { get; set; } = new double[2]; // [longitud, latitud]

    public GeoPoint() { }

    public GeoPoint(double longitude, double latitude)
    {
        Type = "Point";
        Coordinates = new double[] { longitude, latitude };
    }
}
