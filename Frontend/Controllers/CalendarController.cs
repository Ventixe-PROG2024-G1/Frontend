using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class CalendarController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
