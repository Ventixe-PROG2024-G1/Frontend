using Frontend.Models.Event.Requests;
using Frontend.Models.Event.Responses;
using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.InteropServices;

namespace Frontend.Services;

public interface IEventApiService
{
    Task<EventResponseModel?> CreateEventAsync(CreateEventModel createEvent);
    Task<IEnumerable<EventResponseModel>> GetAllEventsAsync();
    Task<EventResponseModel?> GetEventByIdAsync(Guid eventId);
    Task<PaginatedEventResponseModel?> GetPaginatedEventsAsync(int pageNumber = 1, int pageSize = 6, string? categoryNameFilter = null, string? searchTerm = null, string? dateFilter = null, DateTime? specificDateFrom = null, DateTime? specificDateTo = null, string? statusFilter = null);
}

public class EventApiService(HttpClient httpClient) : IEventApiService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<PaginatedEventResponseModel?> GetPaginatedEventsAsync(
        int pageNumber = 1,
        int pageSize = 6,
        string? categoryNameFilter = null,
        string? searchTerm = null,
        string? dateFilter = null,
        DateTime? specificDateFrom = null,
        DateTime? specificDateTo = null,
        string? statusFilter = null
        )
    {
        string requestUrl = "event";

        var queryParams = new Dictionary<string, string?>();

        queryParams["pageNumber"] = pageNumber.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        if (!string.IsNullOrEmpty(categoryNameFilter)) queryParams["categoryNameFilter"] = categoryNameFilter;
        if (!string.IsNullOrEmpty(searchTerm)) queryParams["searchTerm"] = searchTerm;
        if (!string.IsNullOrEmpty(dateFilter)) queryParams["dateFilter"] = dateFilter;
        if (specificDateFrom.HasValue) queryParams["specificDateFrom"] = specificDateFrom.Value.ToString("yyyy-MM-dd");
        if (specificDateTo.HasValue) queryParams["specificDateTo"] = specificDateTo.Value.ToString("yyyy-MM-dd");
        if (!string.IsNullOrEmpty(statusFilter)) queryParams["statusFilter"] = statusFilter;

        var nonNullQueryParams = queryParams
            .Where(kvp => kvp.Value != null)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        if (nonNullQueryParams.Any())
            requestUrl = QueryHelpers.AddQueryString(requestUrl, nonNullQueryParams);

        var response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<PaginatedEventResponseModel>();

        else
            return null;
    }

    public async Task<IEnumerable<EventResponseModel>> GetAllEventsAsync()
    {
        var response = await _httpClient.GetAsync("event/all");

        if (response.IsSuccessStatusCode)
        {
            var events = await response.Content.ReadFromJsonAsync<List<EventResponseModel>?>();
            return events ?? new List<EventResponseModel>();
        }
        else
        {
            return new List<EventResponseModel>();
        }
    }

    public async Task<EventResponseModel?> GetEventByIdAsync(Guid eventId)
    {
        if (eventId == Guid.Empty)
            return null;

        var response = await _httpClient.GetAsync($"event/{eventId}");

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<EventResponseModel?>();
        else
            return null;
    }

    public async Task<EventResponseModel?> CreateEventAsync(CreateEventModel createEvent)
    {
        if (createEvent == null)
            return null;

        var response = await _httpClient.PostAsJsonAsync("event", createEvent);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EventResponseModel?>();
        }
        else
        {
            return null;
        }
    }
}
