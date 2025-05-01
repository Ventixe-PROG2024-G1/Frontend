using AuthenticationLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationLayer.Contexts
{
    public class AuthenticationContext(DbContextOptions<AuthenticationContext> options) : IdentityDbContext<AppUserEntity>(options)
    {
    }
}