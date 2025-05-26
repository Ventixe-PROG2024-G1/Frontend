using Frontend.Models.AppUser;

namespace Frontend.Services
{
    public class AppUserService(HttpClient httpClient) : IAppUserService
    {
        private readonly HttpClient _httpClient = httpClient;

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