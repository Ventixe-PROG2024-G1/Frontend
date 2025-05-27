using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;
[Authorize]

public class GalleryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
