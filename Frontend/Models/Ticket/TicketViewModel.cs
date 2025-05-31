using System.Text.Json.Serialization;

namespace Frontend.Models.Ticket;

public class TicketViewModel
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    [JsonPropertyName("tierDescription")]
    public string TierDescription { get; set; } = string.Empty;
    [JsonPropertyName("tiers")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TicketTier Tier { get; set; }
    public Guid EventId { get; set; }
}
