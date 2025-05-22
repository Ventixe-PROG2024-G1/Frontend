using Frontend.Models.AppUser;

namespace Frontend.Services
{
    public interface IAppUserService
    {
        Task<AppUserResponseRest> GetUserAsync(string id);
    }
}