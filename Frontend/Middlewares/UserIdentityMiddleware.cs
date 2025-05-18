using Frontend.Models.AppUser;
using Frontend.Services;
using Frontend.Stores;
using System.Security.Claims;

namespace Frontend.Middlewares
{
    // AI-genererad kod
    public class UserIdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                using (var scope = serviceProvider.CreateScope())
                {
                    var appUserService = scope.ServiceProvider.GetRequiredService<IAppUserService>();

                    var user = await appUserService.GetUserAsync(userId);

                    if (user != null)
                    {
                        UserStore.CurrentUser = user;
                    }
                }
            }

            await _next(context);
        }
    }
}