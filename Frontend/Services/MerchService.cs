using Frontend.Models.EventDetails;
using Frontend.Models.Ticket;
using System.Net.Http;
using System.Text.Json;

namespace Frontend.Services;

public interface IMerchService
{
    Task<bool> CreateMerchAsync(MerchViewModel merch);
    Task<bool> DeleteMerchAsync(Guid id);
    Task<IEnumerable<MerchViewModel>> GetAllMerchAsync();
    Task<MerchViewModel?> GetMerchByIdAsync(Guid id);
}

public class MerchService(HttpClient httpClient) : IMerchService
{
    private readonly HttpClient _httpClient = httpClient;


    public async Task<IEnumerable<MerchViewModel>> GetAllMerchAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api//Merch");

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var merch = JsonSerializer.Deserialize<List<MerchViewModel>>(json, options);
            return merch ?? new List<MerchViewModel>();
        }

        return new List<MerchViewModel>();

    }

    public async Task<MerchViewModel?> GetMerchByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/Merch/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<MerchViewModel>();
        }
        return null;

    }
    public async Task<bool> CreateMerchAsync(MerchViewModel merch)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Merch", merch);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMerchAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/Merch/{id}");
        return response.IsSuccessStatusCode;
    }

}
