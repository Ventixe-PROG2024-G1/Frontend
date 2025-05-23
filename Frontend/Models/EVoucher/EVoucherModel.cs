namespace Frontend.Models.EVoucher;

public class EVoucherModel
{
    public string Id { get; set; } = "";
    public string TicketId { get; set; } = "";
    public EVoucherTicket Ticket { get; set; } = new();
    public EventSchedule Schedule { get; set; } = new();
    public ProhibitedItems ProhibitedItems { get; set; } = new();
    public TermsConditions TermsConditions { get; set; } = new();
    public VenueMap Map { get; set; } = new();
}

public class EVoucherTicket
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string InvoiceNumber { get; set; } = "";
    public string SeatNumber { get; set; } = "";
    public string Gate { get; set; } = "";
    public string Location { get; set; } = "";
    public string Date { get; set; } = "";
    public string Time { get; set; } = "";
}

public class EventSchedule
{
    public string EventName { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class ProhibitedItems
{
    public int Id { get; set; }
    public List<string> Items { get; set; } = new();
}

public class TermsConditions
{
    public string Text { get; set; } = "";
}

public class VenueMap
{
    public int Id { get; set; }
    public string Url { get; set; } = "";
}
