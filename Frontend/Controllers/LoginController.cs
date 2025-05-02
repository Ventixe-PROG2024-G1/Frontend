using AuthenticationLayer.Entities;
using Frontend.Models.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class LoginController(SignInManager<AppUserEntity> signInManager) : Controller
    {
        private readonly SignInManager<AppUserEntity> _signInManager = signInManager;

        // Viewbag skickar data från controllers till vyer utan att använda en modell.

        public IActionResult Index(string returnUrl = "~/")
        {
            // ReturnUrl sätts till "~/" vilket innebär Home
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = "~/")
        {
            ViewBag.ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var response = await _signInManager.PasswordSignInAsync(
                                        model.Email, model.Password, model.RememberMe, false);

                if (response.Succeeded)
                {
                    return LocalRedirect(ViewBag.ReturnUrl);
                }
            }
            ViewBag.ErrorMessage = "Invalid email or password";
            return View(model);
        }
    }
}