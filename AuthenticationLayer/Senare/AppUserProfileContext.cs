using AuthenticationLayer.Senare.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationLayer.Senare
{
    // Detta är ett separata system
    public class AppUserProfileContext(DbContextOptions<AppUserProfileContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<AppUserProfile> AppUserProfiles { get; set; }

        public DbSet<AppUserProfileAddress> AddressProfiles { get; set; }
    }
}