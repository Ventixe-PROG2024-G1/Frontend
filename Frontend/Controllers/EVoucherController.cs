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
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetByIds/{Invoiceid}/{Eventid}")]
        public async Task<IActionResult> GetByIds(string Invoiceid, string Eventid)
        {
            var Evoucher = await _https.GetFromJsonAsync<EVoucherModel>($"https://localhost:7260/api/Evoucher/{Invoiceid}/{Eventid}");

            if (Evoucher == null)
                return NotFound();

            return Json(Evoucher);
        }
    }
}
