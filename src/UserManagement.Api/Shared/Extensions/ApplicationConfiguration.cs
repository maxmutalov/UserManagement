using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Database;
using UserManagement.Api.Shared.Middlewares;

namespace UserManagement.Api.Shared.Extensions
{
    public static class ApplicationConfiguration
    {
        public static IApplicationBuilder UseUsersSecurityStampChecker(this IApplicationBuilder app)
        {
            app.UseMiddleware<ValidateSecurityStampMiddleware>();

            return app;
        }

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var dataProtectionKeyContext = scope.ServiceProvider.GetRequiredService<DataProtectionKeyContext>();

            //identityDbContext.Database.Migrate();

            dataProtectionKeyContext.Database.Migrate();

            return app;
        }
    }
}
