using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class BookingTourRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string TourType { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public int Infants { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime DepartureDate { get; set; }
    public string PaymentMethod { get; set; }
}
