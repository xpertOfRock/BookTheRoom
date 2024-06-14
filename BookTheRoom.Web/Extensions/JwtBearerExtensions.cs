using BookTheRoom.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookTheRoom.Web.Extencions
{
    public static class JwtBearerExtensions
    {
        public static List<Claim> CreateClaims(this ApplicationUser user, string role)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.Role, user.Email!),
                new(ClaimTypes.Name, user.UserName!)
            };
            return claims;
        }


        public static SigningCredentials CreateSigningCredentials(this IConfiguration configuration)
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
                ),
                SecurityAlgorithms.HmacSha256
            );
        }

        public static JwtSecurityToken CreateJwtToken(this IEnumerable<Claim> claims, IConfiguration configuration)
        {
            var expires = configuration.GetSection("Jwt:Expire").Get<int>();

            return new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: configuration.CreateSigningCredentials()
            );
        }

        public static string CreateRefreshToken()
        {
            var value = new byte[64];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(value);
            return Convert.ToBase64String(value);
        }

        public static ClaimsPrincipal? GetPrincipalFromEpiredToken(this IConfiguration configuration, string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                ValidateLifetime = false
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(
                token,
                tokenValidationParameters,
                out var securityToken
            );

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {                 
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;
        }
    }
}
