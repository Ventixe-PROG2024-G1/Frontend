using Frontend.Data.Contexts;
using Frontend.Data.Entities;
using Frontend.Data.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(x =>
x.UseSqlServer(builder.Configuration.GetConnectionString("ConnStringAuthentication")));

builder.Services.AddIdentity<AppUserEntity, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Roller skapas upp i databasen
await SeedData.SetRolesAsync(app);

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