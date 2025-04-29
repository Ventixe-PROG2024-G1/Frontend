using Frontend.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Data.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUserEntity>(options)
    {
        public DbSet<AppUserAddressEntity> UserAddresses { get; set; }
    }
}