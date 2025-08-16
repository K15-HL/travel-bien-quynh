using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using travel_bien_quynh.Entities;

namespace travel_bien_quynh.Entities;

public class Room : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("adults")]
    public int Adults { get; set; }

    [BsonElement("children")]
    public int Children { get; set; }

    [BsonElement("acreage")]
    public int Acreage { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("rating")]
    public int Rating { get; set; }

    [BsonElement("image")]
    public List<string> Image { get; set; }

    [BsonElement("amenities")]
    public List<string> Amenities { get; set; }

    [BsonElement("status")]
    public bool Status { get; set; }

    [BsonElement("publishedDate")]
    public DateTime PublishedDate { get; set; }
}

