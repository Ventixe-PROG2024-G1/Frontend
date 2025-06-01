using Frontend.Models.AppUser;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Frontend.Services
{
    public class AppUserService(HttpClient httpClient, IConfiguration configuration) : IAppUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _apiKey = configuration.GetValue<string>("SecretKeys:AuthenticationKey")!;

        public async Task<AppUserResponseRest> GetUserAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Invalid user ID.");
                return null;
            }

            var apiUrl = $"https://ventixe-user-provider.azurewebsites.net/api/users/{id}";
           

            try
            {
                return await _httpClient.GetFromJsonAsync<AppUserResponseRest>(apiUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user data: {ex.Message}");
                return null;
            }
        }
    }
}