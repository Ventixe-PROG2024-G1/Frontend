using Frontend.Models;
using Frontend.Models.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace Frontend.Controllers;
[Route("[controller]")]
[Authorize]
public class InvoiceController : Controller
{
    private readonly HttpClient _https;
    private readonly IConfiguration _config;

    public InvoiceController(IHttpClientFactory httpFactory, IConfiguration config)
    {
        _https = httpFactory.CreateClient();
        _config = config;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetInvoices")]
    public async Task<IActionResult> GetInvoices()
    {
        var request = new HttpRequestMessage(HttpMethod.Get,"https://invoice-ventixe-dyhxapdyaqdbcacq.swedencentral-01.azurewebsites.net/api/Invoice");

        var apiKey = _config["SecretKeys:invoiceApiKey"];
        request.Headers.Add("X-API-KEY", apiKey);

        var response = await _https.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Kunde inte hämta fakturor.");

        var invoices = await response.Content.ReadFromJsonAsync<List<InvoiceDto>>();
        return invoices == null ? NotFound() : Json(invoices);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,$"https://invoice-ventixe-dyhxapdyaqdbcacq.swedencentral-01.azurewebsites.net/api/Invoice/{id}");

        var apiKey = _config["SecretKeys:invoiceApiKey"];
        request.Headers.Add("X-API-KEY", apiKey);

        var response = await _https.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Kunde inte hämta faktura.");

        var invoice = await response.Content.ReadFromJsonAsync<InvoiceDetailsDto>();
        return invoice == null ? NotFound() : Json(invoice);
    }
    [HttpGet("GetEmailById/{id}")]
    public async Task<IActionResult> GetEmailById(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,$"https://invoice-ventixe-dyhxapdyaqdbcacq.swedencentral-01.azurewebsites.net/api/Invoice/email/{id}");

        var apiKey = _config["SecretKeys:invoiceApiKey"];
        request.Headers.Add("X-API-KEY", apiKey);

        var response = await _https.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Kunde inte hämta e-postfaktura.");

        return Ok("E-post skickad");
    }

}
