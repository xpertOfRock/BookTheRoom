using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.Web.Extencions;
using BookTheRoom.Web.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace BookTheRoom.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(ApplicationUser user, string role)
        {
            var token = user
                .CreateClaims(role)
                .CreateJwtToken(_configuration);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
