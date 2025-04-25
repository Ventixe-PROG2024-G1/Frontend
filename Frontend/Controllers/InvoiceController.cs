using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class InvoiceController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
