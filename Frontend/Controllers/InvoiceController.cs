using Frontend.Models;
using Frontend.Models.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;
[Route("[controller]")]
[Authorize]
public class InvoiceController : Controller
{
    private readonly HttpClient _https;

    public InvoiceController(IHttpClientFactory httpFactory)
    {
        _https = httpFactory.CreateClient();
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetInvoices")]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await _https.GetFromJsonAsync<List<InvoiceDto>>("https://invoice-ventixe-dyhxapdyaqdbcacq.swedencentral-01.azurewebsites.net/api/Invoice");
        if (invoices == null)
            return NotFound();

        return Json(invoices);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var invoice = await _https.GetFromJsonAsync<InvoiceDetailsDto>($"https://invoice-ventixe-dyhxapdyaqdbcacq.swedencentral-01.azurewebsites.net/api/Invoice/{id}");

        if (invoice == null)
            return NotFound();     

        return Json(invoice);
    }
    [HttpGet("GetEmailById/{id}")]
    public async Task<IActionResult> GetEmailById(string id)
    {
        var invoice = await _https.GetFromJsonAsync<InvoiceDetailsDto>($"https://invoice-ventixe-dyhxapdyaqdbcacq.swedencentral-01.azurewebsites.net/api/Invoice/email{id}");

        if (invoice == null)
            return NotFound();

        return Json(invoice);
    }
}
