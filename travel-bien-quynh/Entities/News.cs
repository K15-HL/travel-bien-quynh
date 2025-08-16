using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using travel_bien_quynh.Entities;

namespace travel_bien_quynh.Entities;

public class News : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("category")]
    public string Category { get; set; }

    [BsonElement ("description")]
    public string Description { get; set; }

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("author")]
    public string Author { get; set; }

    [BsonElement("image")]
    public string Image { get; set; }

    [BsonElement("Link")]
    public string Link { get; set; }

    [BsonElement("publishedDate")]
    public DateTime PublishedDate { get; set; }
    public bool IsActive { get; set; }
}

