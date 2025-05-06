using AuthenticationLayer.Entities;
using Frontend.Models.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class SignUpController : Controller
    {
        #region Step 1 - Set Email

        [HttpGet("signup")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Index(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid email address";
                return View(model);
            }

            // Anrop till AccountServiceProvider
            //var findAccountResponse = await _accountService.FindByEmailAsync(model.Email);
            //if (findAccountResponse.Succeeded)
            //{
            //    ViewBag.ErrorMessage = "Account already exists";
            //    return View(model);
            //}

            // Anrop till VerificaionServiceProvider
            //var verificationResponse = await _verificationService.SendVerificationCodeAsync(model.Email);
            //if (!verificationResponse.Succeeded)
            //{
            //    ViewBag.ErrorMessage = verificationResponse.Error;
            //    return View(model);
            //}

            TempData["Email"] = model.Email;
            return RedirectToAction("AccountVerification");
        }

        #endregion Step 1 - Set Email

        #region Step 2 - Verify Email Address

        [HttpGet("account-verification")]
        public IActionResult AccountVerification()
        {
            if (TempData["Email"] == null)
                return RedirectToAction("Index");

            ViewBag.MaskedEmail = MaskEmail(TempData["Email"]!.ToString()!);
            TempData.Keep("Email");

            return View();
        }

        [HttpPost("account-verification")]
        public async Task<IActionResult> AccountVerification(AccountVerificationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = TempData["Email"]?.ToString();

            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction("Index");

            // Anrop till VerificationServiceProvider
            //var response = await _verificationService.ValidateVerificationCodeAsync(email, model.Code);
            //if (!response.Succeeded)
            //{
            //    ViewBag.ErrorMessage = response.Error;
            //    TempData.Keep("Email");
            //    return View(model);
            //}

            TempData["Email"] = email;
            return RedirectToAction("SetPassword");
        }

        #endregion Step 2 - Verify Email Address

        #region Step 3 - Set Password

        [HttpGet("set-password")]
        public IActionResult SetPassword()
        {
            return View();
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction(nameof(Index));

            var appUser = new AppUserEntity
            {
                UserName = email,
                Email = email,
            };

            // Anrop till AccountServiceProvider
            //var response = await _accountService.CreateAccountAsync(appUser, model.Password);
            //if (!response.Succeeded)
            //{
            //    TempData.Keep("Email");
            //    return View(model);
            //}

            // Skickar tillbaka ID't som genereras till nästa steg för att skapa en koppling.
            //TempData["UserId"] = response.Result;
            return RedirectToAction("ProfileInformation");
        }

        #endregion Step 3 - Set Password

        #region Step 4 - Set Profile Information

        [HttpGet("profile-information")]
        public IActionResult ProfileInformation()
        {
            return View();
        }

        [HttpPost("profile-information")]
        public IActionResult ProfileInformation(ProfileInformationViewModel model)
        {
            return RedirectToAction("Index", "Login");
        }

        #endregion Step 4 - Set Profile Information

        private string MaskEmail(string email)
        {
            var parts = email.Split('@');
            var firstChar = parts[0].First();
            return $"{firstChar}*****@{parts[1]}";
        }
    }
}