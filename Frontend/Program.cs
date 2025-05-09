using AuthenticationLayer.Contexts;
using AuthenticationLayer.Entities;
using AuthenticationLayer.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Frontend.Controllers;
using LocalProfileServiceProvider.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuthenticationContext>(x =>
x.UseSqlServer(builder.Configuration.GetConnectionString("ConnStringAuthentication")));

builder.Services.AddIdentity<AppUserEntity, IdentityRole>(x =>
{
    x.SignIn.RequireConfirmedAccount = true;
    x.Password.RequiredLength = 8;
    x.User.RequireUniqueEmail = true;
}
).AddEntityFrameworkStores<AuthenticationContext>();

// Skapar upp clients
builder.Services.AddGrpcClient<VerificationContract.VerificationContractClient>(x =>
{
    x.Address = new Uri(builder.Configuration["GrpcServices:VerificationService"]!);
});
builder.Services.AddGrpcClient<AccountContract.AccountContractClient>(x =>
{
    x.Address = new Uri(builder.Configuration["GrpcServices:LocalAccountService"]!);
});

builder.Services.AddGrpcClient<ProfileContract.ProfileContractClient>(x =>
{
    x.Address = new Uri(builder.Configuration["GrpcServices:LocalProfileService"]!);
});

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/login";
    x.AccessDeniedPath = "/accessdenied";
    x.Cookie.IsEssential = true;
    x.ExpireTimeSpan = TimeSpan.FromDays(30);
    x.SlidingExpiration = true;
    x.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddHttpClient();

var app = builder.Build();

// Roller skapas upp i databasen
//await SeedData.SetRolesAsync(app);

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();