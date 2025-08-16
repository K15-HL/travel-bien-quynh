public class UpdateRoomRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public int Acreage { get; set; }
    public decimal Price { get; set; }
    public int Rating { get; set; }
    public List<string> Image { get; set; }
    public List<string> Amenities { get; set; }
    public bool Status { get; set; }
}
