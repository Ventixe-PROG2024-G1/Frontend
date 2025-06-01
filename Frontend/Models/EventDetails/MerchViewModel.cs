namespace Frontend.Models.EventDetails;

public class MerchViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid? ImageId { get; set; }
    public Guid EventId { get; set; }
}
