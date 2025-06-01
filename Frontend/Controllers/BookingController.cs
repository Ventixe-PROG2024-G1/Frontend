using Frontend.Models.Booking;
using Frontend.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Frontend.Controllers;

[Authorize]
[Route("[controller]")]
public class BookingController(IHttpClientFactory httpFactory, IConfiguration config) : Controller
{
    private readonly HttpClient _httpClient = httpFactory.CreateClient();
    private readonly IConfiguration _config = config;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var url = $"{_config["RestServices:BookingService"]}/api/bookings";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("booking-api-key", _config["SecretKeys:BookingApiKey"]);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadFromJsonAsync<IEnumerable<BookingModel>>();
            return Ok(json);
        }
        return StatusCode((int)response.StatusCode, "Failed to fetch All Bookings");
    }

    [HttpGet("GetTableData")]
    public async Task<IActionResult> GetTableData([FromQuery] BookingQueryParams queryParams)
    {
        if (UserStore.CurrentUser == null || UserStore.CurrentUser.Role == null || UserStore.CurrentUser.Id == null)
            queryParams.UserId = "None"; // Load no bookings if no user or admin is logged in.
        else if (UserStore.CurrentUser.Role == "Admin")
            queryParams.UserId = null; // Null value loads all bookings for all users.
        else
            queryParams.UserId = UserStore.CurrentUser.Id; // Load the bookings for the current none admin user.

        var queryString = ToQueryString(queryParams);
        var url = $"{_config["RestServices:BookingService"]}/api/bookings/query?{queryString}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("booking-api-key", _config["SecretKeys:BookingApiKey"]);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadFromJsonAsync<BookingQueryResponse>();
            return Ok(json);
        }
        return StatusCode((int)response.StatusCode, "Failed to fetch Booking Table Data");
    }

    // Move to event controller?
    [HttpGet("GetAllEventCategories")]
    public async Task<IActionResult> GetAllEventCategories()
    {
        var url = $"{_config["RestServices:EventService"]}/api/category";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadFromJsonAsync<IEnumerable<EventCategory>>();
            return Ok(json);
        }
        return StatusCode((int)response.StatusCode, "Failed to fetch Event Categories");
    }

    // AI writen helper "function"
    private static string ToQueryString(object obj)
    {
        var properties = from p in obj.GetType().GetProperties()
                         let value = p.GetValue(obj, null)
                         where value != null
                         select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(value.ToString())}";

        return string.Join("&", properties);
    }
}