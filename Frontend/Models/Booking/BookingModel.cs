namespace Frontend.Models.Booking;

public class BookingModel
{
    public string Id { get; set; } = null!;
    public DateTime? Created { get; set; }
    public int TicketQuantity { get; set; }
    public decimal TotalTicketPrice { get; set; }
    public string Status { get; set; } = null!;

    public decimal TicketPrice { get; set; }
    public string? TicketCategory { get; set; }

    public string? UserName { get; set; }

    public string? EventName { get; set; }
    public string? EventCategory { get; set; }

    public string UserId { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string TicketId { get; set; } = null!;
    public string? InvoiceId { get; set; }
    public string? VoucherId { get; set; }
}
