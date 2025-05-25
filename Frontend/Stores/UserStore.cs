using Frontend.Models.AppUser;

namespace Frontend.Stores
{
    public static class UserStore
    {
        public static AppUserResponseRest? CurrentUser { get; set; }
    }
}