using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class BookingRoomRequest
{
    public string Hotel { get; set; }
    public string RoomType { get; set; }

    public string CheckIn { get; set; }

    public string Adults { get; set; }
    public string Checkout { get; set; }

    public int Children { get; set; }
    public string FullName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Nationality { get; set; }

    public string SpecialRequests { get; set; }
    public string PaymentMethod { get; set; }

    public decimal TotalPrice { get; set; }
}
