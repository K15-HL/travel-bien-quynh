using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace travel_bien_quynh.Entities;

public class Information : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("address")]
    public string Address { get; set; }
    [BsonElement("logo")]
    public string Logo { get; set; }
    [BsonElement("favicon")]
    public string Favicon { get; set; }
    [BsonElement("hotline")]
    public string Hotline { get; set; }
    [BsonElement("email")]
    public string Email { get; set; }
    [BsonElement("copyright")]
    public string Copyright { get; set; }
    [BsonElement("country")]
    public string Country { get; set; }
    [BsonElement("nameWebsite")]
    public string NameWebsite { get; set; }

    [BsonElement("facebook")]
    public string Facebook { get; set; }

    [BsonElement("instagram")]
    public string Instagram { get; set; }

    [BsonElement("tikTok")]
    public string TikTok { get; set; }

    [BsonElement("youtube")]
    public string Youtube { get; set; }

    [BsonElement("zalo")]
    public string Zalo{ get; set; }
}
