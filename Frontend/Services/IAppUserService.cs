using Frontend.Models.AppUser;

namespace Frontend.Services
{
    public interface IAppUserService
    {
        Task<AppUser> GetUserAsync(string id);
    }
}