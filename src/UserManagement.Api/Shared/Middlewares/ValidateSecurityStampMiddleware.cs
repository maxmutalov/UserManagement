using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagement.Api.Shared.Entities;

namespace UserManagement.Api.Shared.Middlewares
{
    public class ValidateSecurityStampMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ValidateSecurityStampMiddleware> _logger;

        public ValidateSecurityStampMiddleware(
            RequestDelegate next,
            IServiceScopeFactory scopeFactory,
            ILogger<ValidateSecurityStampMiddleware> logger)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

#pragma warning disable CS8602, CS8604
        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Users security stamp is being validated.");
                using var scope = _scopeFactory.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var tokenSecurityStamp = context.User.FindFirst("SecurityStamp")?.Value;

                if (userId != null && tokenSecurityStamp != null)
                {
                    var user = await userManager.FindByIdAsync(userId);
                    var currentSecurityStamp = await userManager.GetSecurityStampAsync(user);

                    if (currentSecurityStamp != tokenSecurityStamp && user.IsBlocked)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                _logger.LogInformation("Users security stamp validation ended.");
            }

            await _next(context);
        }
#pragma warning restore CS8602, CS8604
    }
}
