using Frontend.Models.Event.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Models.Event.ViewModels;

public enum DateFilterOptionsEnum
{
    Upcoming,
    ThisWeek,
    ThisMonth,
    ThisYear
}

public class EventIndexPageViewModel
{
    public IEnumerable<EventViewModel> Events { get; set; } = new List<EventViewModel>();

    // Paging Features
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    // Filter Features
    public IEnumerable<SelectListItem> CategoryFilterOptions { get; set; } = new List<SelectListItem>();
    public IEnumerable<StatusResponseModel> StatusFilterOptions { get; set; } = new List<StatusResponseModel>();
    public IEnumerable<SelectListItem> DateFilterOptions { get; set; } = new List<SelectListItem>();

    // Current Filter Values
    public string? CurrentCategoryId { get; set; }
    public string? CurrentStatusName { get; set; }
    public string? CurrentSearchTerm { get; set; }
    public string? CurrentDateFilter { get; set; } = "upcoming";
}
