using Frontend.Models.Event.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Models.Event.ViewModels;

public enum DateFilter
{
    Upcoming,
    ThisWeek,
    ThisMonth,
    ThisYear
}

public class EventIndexPageViewModel
{
    public IEnumerable<EventViewModel> Events { get; set; } = [];

    // Paging features
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    // Filter Features
    public IEnumerable<SelectListItem> CategoryFilterOptions { get; set; } = [];
    public IEnumerable<StatusResponseModel> StatusFilterOptions { get; set; } = [];
    public IEnumerable<SelectListItem> DateFilterOptions { get; set; } = [];

    // Current FilterOptions
    public string? CurrentCategoryFilterId { get; set; }
    public string? CurrentStatusFilterName { get; set; }
    public string? CurrentSearchTerm {  get; set; }
    public DateFilter CurrentDateFilter { get; set; } = DateFilter.Upcoming;

    public DateTime? FilterDateFrom { get; set; }
    public DateTime? FilterDateTo { get; set; }
}
