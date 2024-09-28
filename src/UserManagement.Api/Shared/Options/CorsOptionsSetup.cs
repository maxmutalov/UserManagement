using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace UserManagement.Api.Shared.Options
{
    public class CorsOptionsSetup : IConfigureNamedOptions<CorsOptions>
    {
        public void Configure(CorsOptions options)
        {
            options.AddPolicy("BlazorApp", policyBuilder =>
            {
                policyBuilder.WithOrigins("http://localhost:5000");
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowCredentials();
            });
        }

        public void Configure(string? name, CorsOptions options)
        {
            Configure(options);
        }
    }
}
