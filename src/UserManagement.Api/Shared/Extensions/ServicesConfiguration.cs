using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Serilog;
using UserManagement.Api.Database;
using UserManagement.Api.Shared.Authentication;
using UserManagement.Api.Shared.Entities;
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

            services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

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

            services.AddSingleton<JwtHandler>();

            services.AddScoped<UserRepository>();

            return services;
        }

        private static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).Assembly);



            return services;
        }
    }
}
