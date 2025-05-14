using Frontend.Models.AppUser;

namespace Frontend.Services
{
    public class AppUserService(HttpClient httpClient) : IAppUserService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<AppUser> GetUserAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Invalid user ID.");
                return null;
            }

            var apiUrl = $"https://localhost:7286/api/users/{id}";

            try
            {
                return await _httpClient.GetFromJsonAsync<AppUser>(apiUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user data: {ex.Message}");
                return null;
            }
        }
    }
}