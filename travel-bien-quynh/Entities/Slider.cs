using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using travel_bien_quynh.Entities;

namespace travel_bien_quynh.Entities;

public class Slider : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("image")]
    public string Image { get; set; }

    [BsonElement("url")]
    public string Url { get; set; }

    [BsonElement("publishedDate")]
    public DateTime PublishedDate { get; set; }
    public bool Status { get; set; }
}

