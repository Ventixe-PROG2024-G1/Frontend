using Frontend.Models.Event.Responses;

namespace Frontend.Services;

public interface IStatusApiService
{
    Task<IEnumerable<EventStatusModel>> GetEventStatusesAsync();
}

public class StatusApiService(HttpClient httpClient) : IStatusApiService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<EventStatusModel>> GetEventStatusesAsync()
    {
        var statuses = await _httpClient.GetFromJsonAsync<List<EventStatusModel>>("status/eventstatuses");

        return statuses ?? new List<EventStatusModel>();
    }
}
