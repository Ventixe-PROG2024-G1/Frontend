using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Data.Entities
{
    public class AppUserAddressEntity
    {
        [Key, ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public AppUserEntity User { get; set; } = null!;

        public string? StreetName { get; set; }

        public string? PostalCode { get; set; }

        public string? City { get; set; }
    }
}