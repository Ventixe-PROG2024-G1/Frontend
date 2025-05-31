using Frontend.Models.Booking;
using Frontend.Models.Event.Responses;
using Frontend.Models.EventDetails;
using Frontend.Services;
using Frontend.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Frontend.Controllers;

[Route("[controller]")]
public class EventDetailsController(IHttpClientFactory httpFactory, IConfiguration config, IEventApiService eventService) : Controller
{
    private readonly HttpClient _httpClient = httpFactory.CreateClient();
    private readonly IConfiguration _config = config;
    private readonly IEventApiService _eventService = eventService;

    [Route("{eventId}")]
    public async Task<IActionResult> Index(string eventId)
    {     
        if (string.IsNullOrEmpty(eventId))
            return BadRequest("Event ID is required.");

        //var url = $"{_config[$"RestServices:EventService"]}/api/event/{eventId}";
        //var request = new HttpRequestMessage(HttpMethod.Get, url);
        //var response = await _httpClient.SendAsync(request);

        var eventData = await _eventService.GetEventByIdAsync(Guid.Parse(eventId));

        var vm = new EventDetailsPageView
        {
            Event = eventData,
            BookingForm = new AddBookingFormView { EventId = eventId } // prefill event id if needed
        };

        return View(vm);
    }

    [Route("Tickets")]
    public async Task<IActionResult> GetTickets()
    {
        var url = $"{_config[$"RestServices:TicketService"]}/api/tickets";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-API-KEY", _config["SecretKeys:TicketApiKey"]);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<TicketModel>>();
            return Ok(data);
        }
        return StatusCode((int)response.StatusCode, "Failed to fetch Tickets");
    }

    [Route("Tickets/{ticketId}")]
    public async Task<IActionResult> GetTicket(string ticketId)
    {
        var ticket = await GetTicketAsync(ticketId);

        if (ticket == null)
            return StatusCode(404, "Failed to fetch Ticket");

        return Ok(ticket);
    }

    [HttpPost("SubmitBooking")]
    public async Task<IActionResult> SubmitBooking(AddBookingFormView model)
    {

        var eventUrl = $"{_config[$"RestServices:EventService"]}/api/event/{model.EventId}";
        var eventRequest = new HttpRequestMessage(HttpMethod.Get, eventUrl);
        var eventResponse = await _httpClient.SendAsync(eventRequest);

        var eventData = await eventResponse.Content.ReadFromJsonAsync<EventResponseModel>();

        if (!ModelState.IsValid)
        {
            var vm = new EventDetailsPageView
            {
                Event = eventData,
                BookingForm = model
            };

            return View("Index", vm);
        }

        var ticket = await GetTicketAsync(model.TicketId);

        if (ticket == null)
        {
            ModelState.AddModelError("", "Invalid Ticket");
            return View("Index", new EventDetailsPageView { Event = eventData, BookingForm = model });
        }

        if (UserStore.CurrentUser == null)
        {
            ModelState.AddModelError("", "No User Found");
            return View("Index", new EventDetailsPageView { Event = eventData, BookingForm = new AddBookingFormView() });
        }

        var dto = new AddBookingDto
        {
            TicketQuantity = model.TicketQuantity,
            TicketPrice = ticket.Price,
            UserName = $"{UserStore.CurrentUser.FirstName} {UserStore.CurrentUser.LastName}",
            UserEmail = UserStore.CurrentUser.Email,
            UserPhone = UserStore.CurrentUser.Phone,
            UserStreetAddress = UserStore.CurrentUser.StreetAddress,
            UserPostalCode = UserStore.CurrentUser.PostalCode,
            UserCity = UserStore.CurrentUser.City,
            UserId = UserStore.CurrentUser.Id,
            EventId = model.EventId,
            TicketId = model.TicketId
        };

        var json = JsonSerializer.Serialize(dto);

        var url = $"{_config["RestServices:BookingService"]}/api/bookings";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("booking-api-key", _config["SecretKeys:BookingApiKey"]);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            return View("Index", new EventDetailsPageView { Event = eventData, BookingForm = new AddBookingFormView() });

        return View("Index", new EventDetailsPageView { Event = eventData, BookingForm = model });
    }

    private async Task<TicketModel?> GetTicketAsync(string ticketId)
    {
        var url = $"{_config["RestServices:TicketService"]}/api/tickets/{ticketId}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-API-KEY", _config["SecretKeys:TicketApiKey"]);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<TicketModel>();
    }
}
