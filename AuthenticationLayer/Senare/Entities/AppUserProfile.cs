using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationLayer.Senare.Entities
{
    // Detta ska vara ett separat system.
    // AppUserProfile och AppUserProfileAddress ska vara en micro service
    public class AppUserProfile
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string Lastname { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class AppUserProfileAddress
    {
        [Key, ForeignKey(nameof(AppUserProfile))]
        public string AppUserPrfileId { get; set; } = null!;

        public AppUserProfile? AppUserProfile { get; set; } = null!;

        public string StreetName { get; set; }

        public string PostalCode { get; set; }

        public string? City { get; set; }
    }
}