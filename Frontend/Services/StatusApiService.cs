﻿using Frontend.Models.Event.Responses;

namespace Frontend.Services;

public interface IStatusApiService
{
    Task<IEnumerable<StatusResponseModel>> GetEventStatusesAsync();
}

public class StatusApiService(HttpClient httpClient) : IStatusApiService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<StatusResponseModel>> GetEventStatusesAsync()
    {
        var statuses = await _httpClient.GetFromJsonAsync<List<StatusResponseModel>>("api/status/eventstatuses");

        return statuses ?? new List<StatusResponseModel>();
    }
}
