using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;
[Authorize]

public class InboxController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
