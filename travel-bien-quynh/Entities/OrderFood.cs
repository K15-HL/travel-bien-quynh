using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace travel_bien_quynh.Entities;

public class OrderFood : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("hotel")]
    public string hotel { get; set; }

    [BsonElement("room-type")]
    public string RoomType { get; set; }

    [BsonElement("check-in")]
    public string CheckIn { get; set; }

    [BsonElement("check-out")]
    public string Checkout { get; set; }

    [BsonElement("adults")]
    public string Adults { get; set; }

    [BsonElement("children")]
    public int Children { get; set; }

    [BsonElement("fullName")]
    public string FullName { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("phone")]
    public string Phone { get; set; }

    [BsonElement("nationality")]
    public string Nationality { get; set; }

    [BsonElement("special-requests")]
    public string SpecialRequests { get; set; }

    [BsonElement("departureDate")]
    public DateTime DepartureDate { get; set; }

    [BsonElement("paymentMethod")]
    public string PaymentMethod { get; set; }

    [BsonElement("publishedDate")]
    public DateTime PublishedDate { get; set; }
}

