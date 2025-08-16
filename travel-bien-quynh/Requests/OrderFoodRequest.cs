using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class OrderFoodRequest
{
    public string FullName { get;set; }
    public string Email { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }

    public string TourType { get; set; }

    public string Adults { get; set; }

    public int Children6To11 { get; set; }

    public int ChildrenUnder6 { get; set; }

    public DateTime DepartureDate { get; set; }

    public string PaymentMethod { get; set; }
}
