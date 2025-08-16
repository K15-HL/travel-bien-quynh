using MongoDB.Bson.Serialization.Attributes;

public class CreateSliderRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ViewType { get; set; }
    public string Image { get; set; }
    public bool IsActive { get; set; }
}
