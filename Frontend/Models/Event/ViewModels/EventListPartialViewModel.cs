namespace Frontend.Models.Event.ViewModels;

public class EventListPartialViewModel
{
    public IEnumerable<EventViewModel> Events { get; set; } = new List<EventViewModel>();

    // Paging Features
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    // Current Filters
    public string? CurrentCategoryId { get; set; }
    public string? CurrentStatusName { get; set; }
    public string? CurrentSearchTerm { get; set; }
    public string? CurrentDateFilter { get; set; }
}
