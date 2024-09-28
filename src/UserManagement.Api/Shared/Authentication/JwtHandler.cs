using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Api.Shared.Entities;

namespace UserManagement.Api.Shared.Authentication
{
    public class JwtHandler
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<JwtHandler> _logger;

        public JwtHandler(
            IOptions<JwtSettings> jwtSettings,
            UserManager<User> userManager,
            ILogger<JwtHandler> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<String> CreateTokenAsync(User user)
        {
            _logger.LogInformation("JwtHandler.CreateTokenAsync is starting creating JWT token.");
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            _logger.LogInformation("JwtHandler.CreateTokenAsync ended.");
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            //B66B05B5-7E2F-4FAE-BEB2-0A2B2D6C
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<String> GetSecurityStampAsync(User user)
        {
            var securityStamp = await _userManager.GetSecurityStampAsync(user);

            return securityStamp;
        }

        private async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var securityStamp = await GetSecurityStampAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Email!),
                new Claim("SecurityStamp", securityStamp)
            };

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(
            SigningCredentials signingCredentials,
            List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryInMinutes)),
                signingCredentials: signingCredentials
                );

            return tokenOptions;
        }
    }
}
