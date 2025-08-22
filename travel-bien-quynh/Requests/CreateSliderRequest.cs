using MongoDB.Bson.Serialization.Attributes;

public class CreateSliderRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public bool Status { get; set; }
    public string Url { get; set; }
}
