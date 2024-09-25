using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagement.Api.Shared.Entities;

namespace UserManagement.Api.Shared.Middlewares
{
    public class ValidateSecurityStampMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserManager<User> _userManager;

        public ValidateSecurityStampMiddleware(
            RequestDelegate next,
            UserManager<User> userManager)
        {
            _next = next;
            _userManager = userManager;
        }

#pragma warning disable CS8602, CS8604
        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var tokenSecurityStamp = context.User.FindFirst("SecurityStamp")?.Value;

                if (userId != null && tokenSecurityStamp != null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    var currentSecurityStamp = await _userManager.GetSecurityStampAsync(user);

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
