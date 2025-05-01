using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccountVerification()
        {
            return View();
        }

        public IActionResult SetPassword()
        {
            return View();
        }

        public IActionResult ProfileInformation()
        {
            return View();
        }
    }
}