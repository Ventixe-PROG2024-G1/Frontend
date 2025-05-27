using Frontend.Models.Event.QueryParameters;
using Frontend.Models.Event.Responses;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

[Route("[controller]")]
public class EventsController(IEventApiService eventApiService, ICategoryApiService categoryApiService, IStatusApiService statusApiService, IImageApiService imageApiService) : Controller
{
    private readonly IEventApiService _eventApiService = eventApiService;
    private readonly ICategoryApiService _categoryApiService = categoryApiService;
    private readonly IStatusApiService _statusApiService = statusApiService;
    private readonly IImageApiService _imageApiService = imageApiService;
    private const int DefaultPageSize = 6;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetEventList")]
    public async Task<IActionResult> GetEventListData([FromQuery] EventListQueryParameters queryParams)
    {
        var categories = await _categoryApiService.GetEventCategoriesAsync() ?? Enumerable.Empty<CategoryResponseModel>();
        var statuses = await _statusApiService.GetEventStatusesAsync() ?? Enumerable.Empty<StatusResponseModel>();

        string? categoryNameForApi = null;
        if (!string.IsNullOrEmpty(queryParams.CategoryIdFilter) && Guid.TryParse(queryParams.CategoryIdFilter, out Guid categoryGuid))
            categoryNameForApi = categories.FirstOrDefault(c => c.CategoryId == categoryGuid)?.CategoryName;

        var apiDateFilter = (queryParams.DateFilter?.ToLowerInvariant() == "all") ? null : queryParams.DateFilter;
        int actualPageSize = (queryParams.PageSize.HasValue && queryParams.PageSize.Value > 0) ? queryParams.PageSize.Value : DefaultPageSize;

        var paginatedEventsResult = await _eventApiService.GetPaginatedEventsAsync(
            queryParams.PageNumber,
            actualPageSize,
            categoryNameFilter: categoryNameForApi,
            statusFilter: queryParams.StatusFilter,
            searchTerm: queryParams.SearchTerm,
            dateFilter: apiDateFilter
            );

        List<EventResponseModel> enrichedEvents = [];

        if (paginatedEventsResult?.Events != null)
        {
            foreach (var eventDto in paginatedEventsResult.Events)
            {
                if (eventDto.EventImageId.HasValue && eventDto.EventImageId.Value != Guid.Empty)
                {
                    var imageMetaData = await _imageApiService.GetImageMetaDataAsync(eventDto.EventImageId.Value);
                    if (imageMetaData != null)
                    {
                        eventDto.ImageUrl = imageMetaData.ImageUrl;
                    }
                }
                enrichedEvents.Add(eventDto);
            }
        }

        var finalPaginatedResponse = new PaginatedEventResponseModel
        {
            Events = enrichedEvents,
            PageNumber = paginatedEventsResult?.PageNumber ?? queryParams.PageNumber,
            PageSize = paginatedEventsResult?.PageSize ?? actualPageSize,
            TotalCount = paginatedEventsResult?.TotalCount ?? 0,
            TotalPages = paginatedEventsResult?.TotalPages ?? 0,
            HasPreviousPage = paginatedEventsResult?.HasPreviousPage ?? false,
            HasNextPage = paginatedEventsResult?.HasNextPage ?? false
        };

        if (paginatedEventsResult == null)
        {
            finalPaginatedResponse.PageNumber = queryParams.PageNumber;
            finalPaginatedResponse.PageSize = actualPageSize;
        }

        var responseData = new
        {
            Events = finalPaginatedResponse,
            Categories = categories,
            Statuses = statuses
        };

        return Ok(responseData);
    }
}
