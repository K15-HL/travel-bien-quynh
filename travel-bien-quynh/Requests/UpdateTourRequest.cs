using MongoDB.Bson.Serialization.Attributes;
using travel_bien_quynh.Entities;

public class UpdateTourRequest
{
    public string Title { get; set; }
    public string Label { get; set; }
    public double Rating { get; set; }
    public string Duration { get; set; }
    public int TotalReviews { get; set; }
    public string PeopleRange { get; set; }
    public string Schedule { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public List<Schedule> List { get; set; }
    public bool Status { get; set; }
}
