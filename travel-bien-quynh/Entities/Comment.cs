using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace travel_bien_quynh.Entities;

public class Comment : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("fullName")]
    public string FullName { get; set; }

    [BsonDateTimeOptions]
    public DateTime CreatedAt { get; set; }
}
