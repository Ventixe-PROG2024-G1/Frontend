using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class FinancialsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
