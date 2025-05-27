namespace Frontend.Models.Event.ViewModels;

public class EventViewModel
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? CategoryName { get; set; }
    public string? ShortDescription { get; set; }
    public string? Location { get; set; }

    //Date & Time
    public DateTime EventStartDate { get; set; }
    public string FormattedEventDate { get; set; } = string.Empty;
    public string FormattedEventTime {  get; set; } = string.Empty;

    // Ticket Info
    public int MaxAttendees { get; set; } // total tickets
    public int CurrentAttendees { get; set; } // amount sold

    public int TicketsSold => CurrentAttendees;
    public int TicketsRemaining => MaxAttendees - CurrentAttendees;
    public double TicketSalesPercentage => MaxAttendees > 0 ? ((double)CurrentAttendees / MaxAttendees) * 100 : 0;
    public string FormattedTicketSalesPercentage => $"{TicketSalesPercentage}% Ticket Sold";

    public decimal Price { get; set; }
    public string FormattedPrice { get; set; } = string.Empty;

    public bool IsFullyBooked => CurrentAttendees >= MaxAttendees;
    public string? StatusName {  get; set; }
}
