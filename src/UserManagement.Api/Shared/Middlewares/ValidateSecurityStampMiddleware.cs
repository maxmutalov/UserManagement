using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagement.Api.Shared.Entities;

namespace UserManagement.Api.Shared.Middlewares
{
    public class ValidateSecurityStampMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public ValidateSecurityStampMiddleware(
            RequestDelegate next,
            IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

#pragma warning disable CS8602, CS8604
        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                using var scope = _scopeFactory.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var tokenSecurityStamp = context.User.FindFirst("SecurityStamp")?.Value;

                if (userId != null && tokenSecurityStamp != null)
                {
                    var user = await userManager.FindByIdAsync(userId);
                    var currentSecurityStamp = await userManager.GetSecurityStampAsync(user);

                    if (currentSecurityStamp != tokenSecurityStamp)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
            }

            await _next(context);
        }
#pragma warning restore CS8602, CS8604
    }
}
