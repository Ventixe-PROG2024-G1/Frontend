using System.Text.Json.Serialization;

namespace Frontend.Models.Event.Responses;

public class EventResponseModel
{
    [JsonPropertyName("id")]
    public Guid EventId { get; set; }
    public Guid? EventImageId { get; set; }
    public string? ImageUrl { get; set; }
    public string? EventName { get; set; }
    public string? Description { get; set; }
    public CategoryResponseModel? Category { get; set; }
    public DateTime? EventStartDate { get; set; }
    public DateTime? EventEndDate { get; set; }
    public string? Status { get; set; }
    public Guid? LocationId { get; set; }
}
