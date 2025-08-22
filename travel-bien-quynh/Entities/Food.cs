using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using travel_bien_quynh.Entities;

namespace travel_bien_quynh.Entities;

public class Food : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("label")]
    public string Label { get; set; }

    [BsonElement("category")]
    public string Category { get; set; }

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("image")]
    public string Image { get; set; }

    [BsonElement("publishedDate")]
    public DateTime PublishedDate { get; set; }
    public bool Status { get; set; }
}

