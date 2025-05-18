using AuthenticationLayer.Entities;
using Frontend.Models.AppUser;
using Frontend.Models.Login;
using Frontend.Services;
using Frontend.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Frontend.Controllers
{
    public class LoginController(SignInManager<AppUserEntity> signInManager, IAppUserService appUserService) : Controller
    {
        private readonly SignInManager<AppUserEntity> _signInManager = signInManager;
        private readonly IAppUserService _appUserService = appUserService;

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
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var user = await _appUserService.GetUserAsync(userId);

                    if (user != null)
                    {
                        UserStore.CurrentUser = user;
                    }

                    return LocalRedirect(ViewBag.ReturnUrl);
                }
            }
            ViewBag.ErrorMessage = "Invalid email or password";
            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}