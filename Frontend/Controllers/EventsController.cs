using Frontend.Models.Event.QueryParameters;
using Frontend.Models.Event.Requests;
using Frontend.Models.Event.Responses;
using Frontend.Models.Event.ViewModels;
using Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace Frontend.Controllers;

[Route("[controller]")]
//[Authorize]
public class EventsController(IEventApiService eventApiService, ICategoryApiService categoryApiService, IStatusApiService statusApiService, IImageApiService imageApiService, IConfiguration config) : Controller
{
    private readonly IEventApiService _eventApiService = eventApiService;
    private readonly ICategoryApiService _categoryApiService = categoryApiService;
    private readonly IStatusApiService _statusApiService = statusApiService;
    private readonly IImageApiService _imageApiService = imageApiService;
    private readonly IConfiguration _config = config;
    private const int AdjustedPageSize = 12;

    public async Task<IActionResult> Index([FromQuery] EventListQueryParameters queryParams)
    {
        var (rawCategories, rawStatuses) = await GetFilterUIDataAsync();

        var callParams = PrepareApiCallParameters(queryParams, rawCategories);

        var paginatedEventsResult = await _eventApiService.GetPaginatedEventsAsync(
            callParams.PageNumber,
            callParams.PageSize,
            categoryNameFilter: callParams.CategoryNameForApi,
            statusFilter: callParams.StatusNameForApi,
            searchTerm: callParams.SearchTerm,
            dateFilter: callParams.DateFilterForApi
        );

        var eventViewModels = await EnrichAndMapEventDtosAsync(paginatedEventsResult?.Events);

        var viewModel = BuildEventIndexPageViewModel(
            eventViewModels,
            paginatedEventsResult,
            callParams,
            rawCategories,
            rawStatuses
        );

        return View(viewModel);
    }

    [HttpPost("uploadimage")]
    public async Task<IActionResult> UploadEventImage(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Ingen fil vald eller filen är tom.");
        }
        var imageResponse = await _imageApiService.UploadImageAsync(file);
        if (imageResponse == null)
        {
            return StatusCode(500, "Misslyckades med att ladda upp bilden.");
        }
        return Ok(new { imageId = imageResponse.ImageId, imageUrl = imageResponse.ImageUrl });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventModel requestModel, IFormFile? imageFile)
    {
        Guid? uploadedImageId = null;
        if (imageFile != null && imageFile.Length > 0)
        {
            var imageUploadResult = await _imageApiService.UploadImageAsync(imageFile);
            if (imageUploadResult != null && imageUploadResult.ImageId != Guid.Empty)
            {
                uploadedImageId = imageUploadResult.ImageId;
                requestModel.EventImageId = uploadedImageId;
            }
        }

        var createdEventApiResponse = await _eventApiService.CreateEventAsync(requestModel);

        if (createdEventApiResponse != null && createdEventApiResponse.EventId != Guid.Empty)
        {
            return Ok(new { message = "Evenemanget har skapats!", eventData = createdEventApiResponse });
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Misslyckades med att skapa evenemanget." });
        }
    }

    #region GetFilterUIDataAsync Method
    private async Task<(IEnumerable<CategoryResponseModel> Categories, IEnumerable<StatusResponseModel> Statuses)> GetFilterUIDataAsync()
    {
        var categoriesTask = _categoryApiService.GetEventCategoriesAsync();
        var statusesTask = _statusApiService.GetEventStatusesAsync();
        await Task.WhenAll(categoriesTask, statusesTask);
        return (
            categoriesTask.Result ?? Enumerable.Empty<CategoryResponseModel>(),
            statusesTask.Result ?? Enumerable.Empty<StatusResponseModel>()
        );
    }
    #endregion

    #region ApiCallAndViewModelParameters Class
    private class ApiCallAndViewModelParameters
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string? CategoryNameForApi { get; init; }
        public string? StatusNameForApi { get; init; }
        public string? SearchTerm { get; init; }
        public string? DateFilterForApi { get; init; }

        public string? CurrentCategoryIdQuery { get; init; }
        public string? CurrentStatusNameQuery { get; init; }
        public string? CurrentDateFilterQuery { get; init; }
    }
    #endregion

    #region Prepare API Call Parameters
    private ApiCallAndViewModelParameters PrepareApiCallParameters(EventListQueryParameters queryParams, IEnumerable<CategoryResponseModel> categories)
    {
        string? categoryNameForApi = null;
        if (!string.IsNullOrEmpty(queryParams.CategoryIdFilter) && Guid.TryParse(queryParams.CategoryIdFilter, out Guid categoryGuid))
        {
            categoryNameForApi = categories.FirstOrDefault(c => c.CategoryId == categoryGuid)?.CategoryName;
        }

        var apiDateFilter = (queryParams.DateFilter?.ToLowerInvariant() == "all") ? null : queryParams.DateFilter;
        int actualPageSize = (queryParams.PageSize.HasValue && queryParams.PageSize.Value > 0) ? queryParams.PageSize.Value : AdjustedPageSize;

        return new ApiCallAndViewModelParameters
        {
            PageNumber = queryParams.PageNumber <= 0 ? 1 : queryParams.PageNumber,
            PageSize = actualPageSize,
            CategoryNameForApi = categoryNameForApi,
            StatusNameForApi = queryParams.StatusFilter,
            SearchTerm = queryParams.SearchTerm,
            DateFilterForApi = apiDateFilter,

            CurrentCategoryIdQuery = queryParams.CategoryIdFilter,
            CurrentStatusNameQuery = queryParams.StatusFilter,
            CurrentDateFilterQuery = queryParams.DateFilter ?? "upcoming"
        };
    }
    #endregion

    #region Enrich and Map Event DTOs
    private async Task<IEnumerable<EventViewModel>> EnrichAndMapEventDtosAsync(IEnumerable<EventResponseModel>? eventDtos)
    {
        if (eventDtos == null || !eventDtos.Any())
        {
            return Enumerable.Empty<EventViewModel>();
        }

        var mappingTasks = eventDtos.Select(async eventDto =>
        {
            string? imageUrl = eventDto.ImageUrl;
            if (string.IsNullOrEmpty(imageUrl) && eventDto.EventImageId.HasValue && eventDto.EventImageId.Value != Guid.Empty)
            {
                var imageMetaData = await _imageApiService.GetImageMetaDataAsync(eventDto.EventImageId.Value);
                imageUrl = imageMetaData?.ImageUrl;
            }
            return MapToEventViewModel(eventDto, imageUrl);
        });

        return await Task.WhenAll(mappingTasks);
    }
    #endregion

    #region Map to EventViewModel
    private EventViewModel MapToEventViewModel(EventResponseModel eventDto, string? imageUrl)
    {
        var viewModel = new EventViewModel
        {
            EventId = eventDto.EventId,
            EventName = eventDto.EventName ?? "Okänt evenemang",
            ThumbnailUrl = imageUrl,
            CategoryName = eventDto.Category?.CategoryName ?? "Ingen kategori",
            Location = eventDto.LocationId.HasValue ? $"Plats ID: {eventDto.LocationId.Value.ToString().Substring(0, Math.Min(4, eventDto.LocationId.Value.ToString().Length))}..." : "Plats ej angiven",
            EventStartDate = eventDto.EventStartDate ?? DateTime.MinValue,
            StatusName = eventDto.Status,
            ShortDescription = GetShortDescription(eventDto.Description)
        };

        if (eventDto.EventStartDate.HasValue)
        {
            viewModel.FormattedEventDate = eventDto.EventStartDate.Value.ToString("yyyy-MM-dd");
            viewModel.FormattedEventTime = eventDto.EventStartDate.Value.ToString("HH:mm");
        }
        else
        {
            viewModel.FormattedEventDate = "Datum ej satt";
            viewModel.FormattedEventTime = "Tid ej satt";
        }
        return viewModel;
    }
    #endregion

    #region Build PageViewModel
    private EventIndexPageViewModel BuildEventIndexPageViewModel(
        IEnumerable<EventViewModel> eventViewModels,
        PaginatedEventResponseModel? paginatedEventsResult,
        ApiCallAndViewModelParameters callParams,
        IEnumerable<CategoryResponseModel> fetchedCategories,
        IEnumerable<StatusResponseModel> statuses)
    {
        var categorySelectListItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Alla kategorier" } };
        categorySelectListItems.AddRange(fetchedCategories
            .Where(c => c.CategoryName != null)
            .Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName ?? "Okänd kategori",
                Selected = c.CategoryId.ToString() == callParams.CurrentCategoryIdQuery
            }).OrderBy(item => item.Text));

        return new EventIndexPageViewModel
        {
            Events = eventViewModels,
            PageNumber = paginatedEventsResult?.PageNumber ?? callParams.PageNumber,
            PageSize = paginatedEventsResult?.PageSize ?? callParams.PageSize,
            TotalCount = paginatedEventsResult?.TotalCount ?? 0,
            TotalPages = paginatedEventsResult?.TotalPages ?? 0,
            HasPreviousPage = paginatedEventsResult?.HasPreviousPage ?? false,
            HasNextPage = paginatedEventsResult?.HasNextPage ?? false,

            CategoryFilterOptions = categorySelectListItems,
            StatusFilterOptions = statuses,
            DateFilterOptions = GetDateFilterSelectOptions(callParams.CurrentDateFilterQuery),

            CurrentCategoryId = callParams.CurrentCategoryIdQuery,
            CurrentStatusName = callParams.CurrentStatusNameQuery,
            CurrentSearchTerm = callParams.SearchTerm,
            CurrentDateFilter = callParams.CurrentDateFilterQuery
        };
    }
    #endregion

    #region Short Description Helper
    private string GetShortDescription(string? fullDescription)
    {
        if (string.IsNullOrEmpty(fullDescription))
        {
            return "Ingen beskrivning tillgänglig.";
        }
        string plainTextDescription = Regex.Replace(fullDescription, "<.*?>", String.Empty);
        return plainTextDescription.Length <= 100
            ? plainTextDescription
            : plainTextDescription.Substring(0, 97) + "...";
    }
    #endregion

    #region Date Filter Options
    private IEnumerable<SelectListItem> GetDateFilterSelectOptions(string? selectedDateFilterValue)
    {
        var currentFilter = selectedDateFilterValue?.ToLowerInvariant() ?? "upcoming";
        var options = new List<SelectListItem>
            {
                new SelectListItem { Value = "all", Text = "Alla datum", Selected = currentFilter == "all" },
                new SelectListItem { Value = "upcoming", Text = "Kommande", Selected = currentFilter == "upcoming" },
                new SelectListItem { Value = "thisweek", Text = "Denna vecka", Selected = currentFilter == "thisweek" },
                new SelectListItem { Value = "thismonth", Text = "Denna månad", Selected = currentFilter == "thismonth" },
                new SelectListItem { Value = "thisyear", Text = "Detta år", Selected = currentFilter == "thisyear" },
                new SelectListItem { Value = "past", Text = "Tidigare", Selected = currentFilter == "past" }
            };
        return options;
    }
    #endregion
}
