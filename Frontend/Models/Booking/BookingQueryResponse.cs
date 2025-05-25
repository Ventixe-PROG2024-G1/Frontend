namespace Frontend.Models.Booking;

public class BookingQueryResponse
{
    public IEnumerable<BookingModel> Bookings { get; set; } = null!;
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
}
