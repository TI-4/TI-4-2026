using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Incident.Domain.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
public class LostObjectMap : BsonClassMap<LostObject> {
    public LostObjectMap()
    {
        AutoMap();
        MapIdMember(x => x.Id)
                    .SetSerializer(
                        new StringSerializer(BsonType.ObjectId)
                    );
        MapMember(x => x.StructureRefId)
                    .SetSerializer(
                        new GuidSerializer(BsonType.String)
                    );

        MapMember(x => x.Title)
                    .SetElementName("title");

        MapMember(x => x.Description)
                    .SetElementName("description");

        MapMember(x => x.Status)
                    .SetElementName("status")
                    .SetSerializer(
                        new  EnumSerializer<Objectenum>(BsonType.String)
                    );

        MapMember(x => x.ReportedAt)
                    .SetElementName("date_lost_object");

        MapMember(x => x.PhotoUrl)
                    .SetElementName("photo_url");

    }

}
