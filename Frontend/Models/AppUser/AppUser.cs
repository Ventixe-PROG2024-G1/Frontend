namespace Frontend.Models.AppUser
{
    public class AppUser
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Image { get; set; }

        public string? Role { get; set; }

        public string? StreetAddress { get; set; }

        public string? PostalCode { get; set; }

        public string? City { get; set; }
    }
}