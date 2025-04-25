using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class InboxController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
