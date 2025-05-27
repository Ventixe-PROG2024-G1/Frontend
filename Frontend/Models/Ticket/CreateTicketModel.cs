namespace Frontend.Models.Ticket;

public enum TicketTier
{
    BackstageAccess, VipLounge, Diamond, Platinum, Gold, Silver, Bronze, GeneralAdmission
}

public class CreateTicketModel
{
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string TierDescription { get; set; } = string.Empty;
    public TicketTier Tier { get; set; }
    public string? EventId { get; set; }
}
