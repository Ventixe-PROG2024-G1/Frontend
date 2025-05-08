using AuthenticationLayer.Entities;
using Frontend.Models.SignUp;
using LocalProfileServiceProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class SignUpController(VerificationContract.VerificationContractClient verificationClient,
                 AccountContract.AccountContractClient accountServiceClient,
                 ProfileContract.ProfileContractClient profileServiceClient) : Controller
    {
        private readonly VerificationContract.VerificationContractClient _verificationServiceClient = verificationClient;
        private readonly AccountContract.AccountContractClient _accountServiceClient = accountServiceClient;
        private readonly ProfileContract.ProfileContractClient _profileServiceClient = profileServiceClient;

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

            var findEmailRequest = new FindByEmailRequest { Email = model.Email };
            var findEmailResponse = await _accountServiceClient.FindByEmailAsync(findEmailRequest);
            if (findEmailResponse.Success)
            {
                ViewBag.ErrorMessage = "Account already exists";
                return View(model);
            }
            var verificationRequest = new SendVerificationCodeRequest { Email = model.Email };
            var verificationResponse = await _verificationServiceClient.SendVerificationCodeAsync(verificationRequest);
            if (!verificationResponse.Succeeded)
            {
                ViewBag.ErrorMessage = verificationResponse.Error;
                return View(model);
            }

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

            var verifyRequest = new VerifyVerificationCodeRequest { Email = email, Code = model.Code };
            var verifyResponse = await _verificationServiceClient.VerifyVerificationCodeAsync(verifyRequest);

            if (!verifyResponse.Succeeded)
            {
                ViewBag.ErrorMessage = verifyResponse.Error;
                TempData.Keep("Email");
                return View(model);
            }
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

            var accountRequest = new CreateAccountRequest { Email = email, Password = model.Password };
            var accountResponse = await _accountServiceClient.CreateAccountAsync(accountRequest);
            if (!accountResponse.Success)
            {
                TempData.Keep("Email");
                return View(model);
            }

            // Skickar tillbaka ID't som genereras till nästa steg för att skapa en koppling.
            TempData["UserId"] = accountResponse.Result;
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
        public async Task<IActionResult> ProfileInformation(ProfileInformationViewModel model)
        {
            var userId = TempData["UserId"]!.ToString();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction(nameof(Index));
            }

            // Anrop till ImageMicroService som sparar ner bilden i Azure blob storage och returnerar ProfilePictureUrl

            //Anrop till LocalProfileServiceProvider för att spara profil kopplad till UserId

            var profileRequest = new CreateProfileRequest
            {
                Id = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                StreetAddress = model.StreetAddress,
                ZipCode = model.ZipCode,
                City = model.City,
                ProfilePictureUrl = model.ProfilePictureUrl ?? "",
            };

            var profileResponse = await _profileServiceClient.CreateProfileAsync(profileRequest);
            if (!profileResponse.Succeeded)
            {
                TempData.Keep("UserId");
                return View(model);
            }

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