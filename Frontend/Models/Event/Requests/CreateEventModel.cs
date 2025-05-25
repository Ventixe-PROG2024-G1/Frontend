namespace Frontend.Models.Event.Requests;

public class CreateEventModel
{
    public string EventName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? EventImageId { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? LocationId { get; set; }
}
