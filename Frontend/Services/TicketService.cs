using Frontend.Models.Event.Responses;
using Frontend.Models.Ticket;
using System.Text.Json;

namespace Frontend.Services;

public interface ITicketService
{
    Task<bool> CreateTicketAsync(TicketViewModel ticket);
    Task<bool> DeleteTicketAsync(Guid id);
    Task<IEnumerable<TicketViewModel>> GetAllTicketsAsync();
    Task<TicketViewModel?> GetTicketsByIdAsync(Guid id);
}

public class TicketService(HttpClient httpClient, IConfiguration config) : ITicketService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _apiKey = config["SecretKeys:TicketApiKey"]!;

    public async Task<IEnumerable<TicketViewModel>> GetAllTicketsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/tickets");
        request.Headers.Add("X-API-KEY", _apiKey);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync(); 
            
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var tickets = JsonSerializer.Deserialize<List<TicketViewModel>>(json, options);
            return tickets ?? new List<TicketViewModel>();
        }

        return new List<TicketViewModel>();

    }

    public async Task<TicketViewModel?> GetTicketsByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/tickets/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TicketViewModel>();
        }
        return null;

    }
    public async Task<bool> CreateTicketAsync(TicketViewModel ticket)
    {
        var response = await _httpClient.PostAsJsonAsync("api/tickets", ticket);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTicketAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/tickets/{id}");
        return response.IsSuccessStatusCode;
    }

}
