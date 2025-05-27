using Frontend.Models.EVoucher;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [Route("[controller]")]
    public class EVoucherController : Controller
    {
        private readonly HttpClient _https;

        public EVoucherController(IHttpClientFactory httpFactory)
        {
            _https = httpFactory.CreateClient();
        }
        [HttpGet("GetByIds/{Invoiceid}/{Eventid}")]
        public async Task<IActionResult> GetByIds(string Invoiceid, string Eventid)
        {
            var Evoucher = await _https.GetFromJsonAsync<EVoucherModel>($"https://e-voucher-ventixe-csgvatffg9b7gacy.swedencentral-01.azurewebsites.net/api/Evoucher/{Invoiceid}/{Eventid}");

            if (Evoucher == null)
                return NotFound();

            return View("VoucherDetails", Evoucher);
        }
    }
}
