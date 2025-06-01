using System.Text.Json.Serialization;

namespace Frontend.Models.Ticket;

public class TicketViewModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    [JsonPropertyName("tierDescription")]
    public string TierDescription { get; set; } = string.Empty;
    [JsonPropertyName("tiers")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TicketTier Tier { get; set; }
    [JsonPropertyName("eventId")]
    public Guid EventId { get; set; }
}
