using Frontend.Models.Responses;
using Frontend.Models.SignUp;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class SignUpController(HttpClient httpClient) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;

        private readonly string _accountServiceUrl = "https://local-account-f0epfuc6b6amgwdd.swedencentral-01.azurewebsites.net";
        private readonly string _verificationServiceUrl = "https://verification-ahbgdfdbceg9dccm.swedencentral-01.azurewebsites.net";
        private readonly string _profileServiceUrl = "https://local-profile-ggh3dsf7cwhhhkfa.swedencentral-01.azurewebsites.net";

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

            var findEmailRequest = new { Email = model.Email };
            var findEmailResponse = await GetRequest<AccountServiceResultRest>($"{_accountServiceUrl}/find-by-email/{model.Email}");
            if (findEmailResponse?.Success == true)
            {
                ViewBag.ErrorMessage = "Account already exists";
                return View(model);
            }

            var verificationRequest = new { Email = model.Email };
            var verificationResponse = await PostRequest<VerificationResponseRest>($"{_verificationServiceUrl}/send-verification-code", verificationRequest);
            if (!verificationResponse?.Succeeded == true)
            {
                ViewBag.ErrorMessage = verificationResponse?.Error;
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

            var verifyRequest = new { Email = email, Code = model.Code };
            var verifyResponse = await PostRequest<VerificationResponseRest>($"{_verificationServiceUrl}/verify-verification-code", verifyRequest);

            if (!verifyResponse?.Succeeded == true)
            {
                ViewBag.ErrorMessage = verifyResponse?.Error;
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

            var accountRequest = new { Email = email, Password = model.Password };
            var accountResponse = await PostRequest<AccountServiceResultRest>($"{_accountServiceUrl}/create-account", accountRequest);
            if (!accountResponse?.Success == true)
            {
                TempData.Keep("Email");
                return View(model);
            }

            // Skickar tillbaka ID't som genereras till nästa steg för att skapa en koppling.
            TempData["UserId"] = accountResponse?.Result;
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

            var profileRequest = new
            {
                Id = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                StreetAddress = model.StreetAddress,
                ZipCode = model.ZipCode,
                City = model.City,
                ProfilePictureUrl = model.ProfilePictureUrl ?? "",
                Phone = model.Phone,
            };

            var profileResponse = await PostRequest<ProfileResponseRest>($"{_profileServiceUrl}/create-profile", profileRequest);
            if (!profileResponse?.Succeeded == true)
            {
                TempData.Keep("UserId");
                return View(model);
            }

            return RedirectToAction("Index", "Login");
        }

        #endregion Step 4 - Set Profile Information

        // AI-genererad kod
        public async Task<T?> PostRequest<T>(string url, object requestData)
        {
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode) return default;

            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(responseString, options);
        }

        // AI-genererad kod
        public async Task<T?> GetRequest<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return default;

            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(responseString, options);
        }

        private string MaskEmail(string email)
        {
            var parts = email.Split('@');
            var firstChar = parts[0].First();
            return $"{firstChar}*****@{parts[1]}";
        }
    }
}