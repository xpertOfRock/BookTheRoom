using Api.Contracts.Account;
using Api.Contracts.Token;
using Application.UseCases.Commands.Apartment;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ISender _sender;
        private readonly IEmailService _emailService;
        private readonly IPhotoService _photoService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ISender sender,
            IEmailService emailService,
            IPhotoService photoService,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
        {
            _sender = sender;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _photoService = photoService;
            _contextAccessor = contextAccessor;
        }
        [HttpPut("Edit")]
        [Authorize]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Edit([FromForm] EditProfileRequest request)
        {
            var userId = _contextAccessor.HttpContext!.User.GetUserId();

            if (userId is null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return BadRequest();

            string imageUrl = string.Empty;

            if (request.Image is not null)
            {
                if(user.Image is null) await _photoService.DeletePhotoAsync(user.Image);

                using var stream = request.Image.OpenReadStream();

                var resultImage = await _photoService
                    .AddPhotoAsync(request.Image.Name, stream);

                imageUrl = resultImage.Url.ToString();
                user.Image = imageUrl;
            }


            user.Image = imageUrl == string.Empty ? user.Image : imageUrl;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            try
            {
                var updateUserResult = await _userManager.UpdateAsync(user);
                if (!updateUserResult.Succeeded) return BadRequest("Could not update the user.");
            }
            catch (Exception)
            {
                if(imageUrl != string.Empty) await _photoService.DeletePhotoAsync(imageUrl);
                throw;
            }      

            var fullName = $"{user.FirstName} {user.LastName}".Trim();

            var updateUserDataInApartmentsRequest = new UpdateUserDataInUserApartmentsRequest(fullName, user.Email, user.PhoneNumber);       
            
            var updateUserApartmentsResult = await _sender.Send(new UpdateUserDataInUserApartmentsCommand(userId, updateUserDataInApartmentsRequest));

            return Ok(user);
        }
        [HttpPost("Login")]
        [EnableRateLimiting("SlidingModify")]
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

            var token = GenerateJwtToken(user);
            var refreshToken = await GenerateAndStoreRefreshToken(user);
            return Ok(new { token, refreshToken, user });
        }

        [HttpPost("Register")]
        [EnableRateLimiting("SlidingModify")]
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
                BirthDate = request.BirthDate,
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

            const string subject = "Registration";
            const string body = "Thanks for choosing Book The Room! Hope you will be satisfied with our service!";
            _emailService.SendEmail(newUser.Email, subject, body);

            var token = GenerateJwtToken(newUser);
            var refreshToken = await GenerateAndStoreRefreshToken(newUser);

            return Ok(new { token, refreshToken, newUser });

        }

        [HttpPost("Logout")]
        [Authorize]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                await RemoveTokens(user);
            }
            return Ok(new { message = "Successfully logged out." });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.Token); //recieving expired access token

            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token."); // if token does not contain any claims return code 400
            }

            var username = principal.Identity!.Name;

            var user = await _userManager.FindByNameAsync(username!); // finding user by the 'username' contained in the claims in jwt

            if (user == null)
            {
                return BadRequest("User not found."); // if user not found return code 400
            }

            var storedRefreshToken = await GetStoredRefreshToken(user);

            if (storedRefreshToken != request.RefreshToken || storedRefreshToken.IsNullOrEmpty())
            {
                return BadRequest("Invalid refresh token."); // if user's refresh token was not found in DB or is not equal to the one that contains in DB return code 400 
            }

            var refreshTokenExpiryTimeString = await _userManager.GetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshTokenExpiryTime");

            if (refreshTokenExpiryTimeString != null && DateTime.Parse(refreshTokenExpiryTimeString) < DateTime.UtcNow)
            {
                return BadRequest("Refresh token has expired.");
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
                new(ClaimTypes.NameIdentifier, user.Id),
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

            var token = new JwtSecurityToken
            (
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);      
        }

        private async Task<string> GenerateAndStoreRefreshToken(ApplicationUser user)
        {
            var refreshToken = GenerateRefreshToken();

            var result = await _userManager.SetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshToken", refreshToken);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to save refresh token.");
            }

            await _userManager.SetAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshTokenExpiryTime", DateTime.UtcNow.AddMonths(1).ToString());

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

        private async Task RemoveTokens(ApplicationUser user)
        {
            var refresh = await _userManager.RemoveAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshToken");
            var expireTime = await _userManager.RemoveAuthenticationTokenAsync(user, "BookTheRoomWeb", "RefreshTokenExpiryTime");
        }
    }
}
