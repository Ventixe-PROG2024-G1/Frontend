namespace Frontend.Models.Event.QueryParameters;

public class EventListQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int? PageSize { get; set; }
    public string? CategoryIdFilter { get; set; }
    public string? StatusFilter { get; set; }
    public string? SearchTerm { get; set; }
    public string? DateFilter { get; set; }
}
