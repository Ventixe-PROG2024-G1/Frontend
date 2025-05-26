namespace Frontend.Models.Booking;

public class BookingQueryParams
{
    //Paging
    private const int MAX_PAGE_SIZE = 24;
    private int _pageSize = 8;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
    }
    public int PageNumber { get; set; } = 1;

    //Sorting
    public string SortBy { get; set; } = "Created";
    public bool SortDescending { get; set; } = false;

    //Filtering
    public int? Days { get; set; }
    public string? UserId { get; set; }
    public string? Status { get; set; }
    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
}
