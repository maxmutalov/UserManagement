using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace UserManagement.Api.Shared.Options
{
    public class IdentityOptionsSetup : IConfigureNamedOptions<IdentityOptions>
    {
        public void Configure(IdentityOptions options)
        {
            options.Password.RequiredLength = 1;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.AllowedUserNameCharacters = String.Empty;
        }

        public void Configure(string? name, IdentityOptions options)
        {
            Configure(options);
        }
    }
}
