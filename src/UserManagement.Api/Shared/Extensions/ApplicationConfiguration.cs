using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Database;
using UserManagement.Api.Shared.Middlewares;

namespace UserManagement.Api.Shared.Extensions
{
    public static class ApplicationConfiguration
    {
        public static IApplicationBuilder CheckUsersSecurityStamp(this IApplicationBuilder app)
        {
            app.UseMiddleware<ValidateSecurityStampMiddleware>();

            return app;
        }

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
