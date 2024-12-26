using Api.Contracts.Account;
using Api.Contracts.Token;
using Application.Interfaces;
using Core.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailService emailService,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthorizeRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.EmailOrUsername) ??
                       await _userManager.FindByNameAsync(request.EmailOrUsername);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid login attempt." });
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordCheck)
            {
                return BadRequest(new { message = "Invalid login attempt." });
            }

            await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            var token = GenerateJwtToken(user);
            var refreshToken = await GenerateAndStoreRefreshToken(user);
            return Ok(new { token, refreshToken, user });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            var existingUserByUsername = await _userManager.FindByNameAsync(request.Username);
            if (existingUserByUsername != null)
            {
                return BadRequest(new { message = "Username is already taken." });
            }

            var newUser = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate.ToString(),
                Role = UserRole.User,
                Orders = new List<Order>(),
                Apartments = new List<Apartment>()
            };

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createUserResult.Succeeded)
            {
                return BadRequest(createUserResult.Errors);
            }

            await _userManager.AddToRoleAsync(newUser, UserRole.User);
            await _signInManager.PasswordSignInAsync(newUser, request.Password, false, false);

            const string subject = "Registration";
            const string body = "Thanks for choosing Book The Room! Hope you will be satisfied with our service!";
            _emailService.SendEmail(newUser.Email, subject, body);

            var token = GenerateJwtToken(newUser);
            var refreshToken = await GenerateAndStoreRefreshToken(newUser);

            return Ok(new { token, refreshToken, newUser });

        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await RemoveRefreshToken(user);
            }
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Successfully logged out." });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token.");
            }

            var username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshToken");
            var refreshTokenExpiryTimeString = await _userManager.GetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshTokenExpiryTime");

            if (refreshTokenExpiryTimeString != null && DateTime.Parse(refreshTokenExpiryTimeString) < DateTime.UtcNow)
            {
                return BadRequest("Refresh token has expired.");
            }

            if (storedRefreshToken != request.RefreshToken)
            {
                return BadRequest("Invalid refresh token.");
            }

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = await GenerateAndStoreRefreshToken(user);

            return Ok(new { token = newToken, refreshToken = newRefreshToken });
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName ?? "null"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Email, user.Email ?? "null"),
                new(ClaimTypes.HomePhone, user.PhoneNumber ?? "null"),
                new(ClaimTypes.GivenName, user.FirstName ?? "null"),
                new(ClaimTypes.Surname, user.LastName ?? "null")
            };

            var jwtSettings = _configuration.GetSection("Jwt");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.Now.AddMinutes(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task<string> GenerateAndStoreRefreshToken(ApplicationUser user)
        {
            var refreshToken = GenerateRefreshToken();

            var result = await _userManager.SetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshToken", refreshToken);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to save refresh token.");
            }

            await _userManager.SetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshTokenExpiryTime", DateTime.UtcNow.AddDays(7).ToString());

            return refreshToken;
        }

        private async Task<string> GetStoredRefreshToken(ApplicationUser user)
        {
            var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshToken");

            if (refreshToken == null)
            {
                throw new Exception("Refresh token not found.");
            }

            return refreshToken;
        }

        private async Task RemoveRefreshToken(ApplicationUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshToken");
            await _userManager.RemoveAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshTokenExpiryTime");
        }
    }
}
