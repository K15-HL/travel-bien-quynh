using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using travel_bien_quynh.Entities;

namespace travel_bien_quynh.Entities;

public class Tour : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("label")]
    public string Label { get; set; }

    [BsonElement("Rating")]
    public double Rating { get; set; }
    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("duration")]
    public string Duration { get; set; }

    [BsonElement("totalReviews")]
    public int TotalReviews { get; set; }
    [BsonElement("peopleRange")]

    public string PeopleRange { get; set; }
    [BsonElement("Schedule")]
    public string Schedule { get; set; }

    [BsonElement("list")]
    public List<Schedule> List { get; set; }

    [BsonElement ("originalPrice")]
    public decimal OriginalPrice { get; set; }

    [BsonElement("discountedPrice")]
    public decimal DiscountedPrice { get; set; }

    [BsonElement("image")]
    public string Image { get; set; }

    [BsonElement("publishedDate")]
    public DateTime PublishedDate { get; set; }
    public bool IsActive { get; set; }
}

public class Schedule
{
    [BsonElement("day")]
    public int Day { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("activities")]
    public List<string> Activities { get; set; }
}