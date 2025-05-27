using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;
[Authorize]

public class FinancialsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
