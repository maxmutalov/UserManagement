using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserManagement.Api.Database;
using UserManagement.Api.Shared.Authentication;
using UserManagement.Api.Shared.Entities;
using UserManagement.Api.Shared.Middlewares;
using UserManagement.Api.Shared.Options;
using UserManagement.Api.Shared.Repositories;

namespace UserManagement.Api.Shared.Extensions
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddAuth(configuration);
            services.AddOtherServices();

            return services;
        }

        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString, options =>
                {
                    options.MigrationsHistoryTable(HistoryRepository.DefaultTableName);
                })
                .UseSnakeCaseNamingConvention();
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddDbContext<DataProtectionKeyContext>(options =>
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

            services.AddDataProtection()
                .PersistKeysToDbContext<DataProtectionKeyContext>()
                .SetApplicationName("UserManagement.Api");

            services.ConfigureOptions<IdentityOptionsSetup>();

            return services;
        }

        private static IServiceCollection AddAuth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer();

            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));

            services.ConfigureOptions<JwtOptionsSetup>();

            services.TryAddScoped<JwtHandler>();

            services.TryAddScoped<UserRepository>();

            return services;
        }

        private static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddCors();

            services.ConfigureOptions<CorsOptionsSetup>();

            return services;
        }
    }
}
