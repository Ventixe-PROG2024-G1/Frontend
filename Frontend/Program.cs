using AuthenticationLayer.Contexts;
using AuthenticationLayer.Entities;
using Frontend.Middlewares;
using Frontend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
//builder.Services.AddGrpcClient<VerificationContract.VerificationContractClient>(x =>
//{
//    x.Address = new Uri(builder.Configuration["GrpcServices:VerificationService"]!);
//});
//builder.Services.AddGrpcClient<AccountContract.AccountContractClient>(x =>
//{
//    x.Address = new Uri(builder.Configuration["GrpcServices:LocalAccountService"]!);
//});

//builder.Services.AddGrpcClient<ProfileContract.ProfileContractClient>(x =>
//{
//    x.Address = new Uri(builder.Configuration["GrpcServices:LocalProfileService"]!);
//});

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/login";
    x.LogoutPath = "/login";
    x.AccessDeniedPath = "/accessdenied";
    x.Cookie.IsEssential = true;
    x.ExpireTimeSpan = TimeSpan.FromDays(30);
    x.SlidingExpiration = true;
    x.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddScoped<IAppUserService, AppUserService>();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient<IEventApiService, EventApiService>(client =>
{
    var baseAdress = builder.Configuration["RestServices:EventService"];
    if (string.IsNullOrEmpty(baseAdress))
    {
        throw new InvalidOperationException("EventsService URL not configured in RestService:EventService");
    }
    client.BaseAddress = new Uri(baseAdress);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<IStatusApiService, StatusApiService>(client =>
{
    var baseAdress = builder.Configuration["RestServices:EventService"];
    if (string.IsNullOrEmpty(baseAdress))
    {
        throw new InvalidOperationException("EventsService URL not configured in RestService:EventService");
    }
    client.BaseAddress = new Uri(baseAdress);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<ICategoryApiService, CategoryApiService>(client =>
{
    var baseAdress = builder.Configuration["RestServices:EventService"];
    if (string.IsNullOrEmpty(baseAdress))
    {
        throw new InvalidOperationException("EventsService URL not configured in RestService:EventService");
    }
    client.BaseAddress = new Uri(baseAdress);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<IImageApiService, ImageApiService>(client =>
{
    var baseAdress = builder.Configuration["RestServices:EventService"];
    if (string.IsNullOrEmpty(baseAdress))
    {
        throw new InvalidOperationException("ImageService URL not configured in RestService:ImageService");
    }
    client.BaseAddress = new Uri(baseAdress);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


var app = builder.Build();

// Roller skapas upp i databasen
//await SeedData.SetRolesAsync(app);

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<UserIdentityMiddleware>();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();