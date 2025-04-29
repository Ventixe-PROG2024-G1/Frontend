using Microsoft.AspNetCore.Identity;

namespace Frontend.Data.Entities
{
    public class AppUserEntity : IdentityUser
    {
        // Id genereras automatiskt av guid (textsträng istället för int)

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? RoleName { get; set; }

        public string? Image { get; set; }

        public AppUserAddressEntity? Address { get; set; }
    }
}