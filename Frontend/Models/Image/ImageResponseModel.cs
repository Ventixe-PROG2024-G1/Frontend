namespace Frontend.Models.Image;

public class ImageResponseModel
{
    public Guid ImageId { get; set; }
    public string? ImageUrl { get; set; }
    public string? ContentType { get; set; }
}