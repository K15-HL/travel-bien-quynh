using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace travel_bien_quynh.Entities;

public class User : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("email")]
    public string Email { get; set; }
    [BsonElement("username")]
    public string Username { get; set; }
    [BsonElement("password")]
    public string Password { get; set; }

    [BsonElement("money")]
    public double Money { get; set; }

    [BsonElement("role")]
    public string Role { get; set; }

    [BsonElement("typeaccount")]
    public string TypeAccount { get; set; }

    [BsonElement("ban")]
    public bool Ban { get; set; }

    [BsonDateTimeOptions]
    public DateTime CreatedAt { get; set; }
}
