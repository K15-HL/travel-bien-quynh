using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace travel_bien_quynh.Entities;

public class BookingTour : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("fullname")]
    public string FullName{ get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("phone")]
    public string Phone { get; set; }

    [BsonElement("address")]
    public string Address { get; set; }

    [BsonElement("tourType")]
    public string TourType { get; set; }

    [BsonElement("adults")]
    public int Adults { get; set; }

    [BsonElement("children")]
    public int Children { get; set; }

    [BsonElement("infants")]
    public int Infants { get; set; }

    [BsonElement("departureDate")]
    public DateTime DepartureDate { get; set; }

    [BsonElement("paymentMethod")]
    public string PaymentMethod { get; set; }

    [BsonElement("totalPrice")]
    public decimal TotalPrice { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}

