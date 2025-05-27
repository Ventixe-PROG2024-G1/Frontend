namespace Frontend.Models.EventDetails;

public class TicketModel
{
    public string Id { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Tiers { get; set; } = null!;
    public string? TierDescription { get; set; }
}
