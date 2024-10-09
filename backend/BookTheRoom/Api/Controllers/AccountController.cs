using Api.Contracts.Account;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthorizeRequest request)
        {

            var thisUser = await _userManager.FindByEmailAsync(request.EmailOrUsername) ??
                            await _userManager.FindByNameAsync(request.EmailOrUsername);

            if (thisUser == null)
            {
                return BadRequest();
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(thisUser, request.Password);

            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(thisUser, request.Password, false, false);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                return BadRequest();
            }

            user = await _userManager.FindByNameAsync(request.Username);

            if (user != null)
            {
                return BadRequest();
            }

            var newUser = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Username,
                PhoneNumber = request.PhoneNumber,
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, request.Password);

            if (!newUserResponse.Succeeded)
            {
                return BadRequest(newUserResponse.Errors);
            }

            await _userManager.AddToRoleAsync(newUser, UserRole.Admin);
            await _signInManager.PasswordSignInAsync(newUser, request.Password, false, false);

            const string subject = "Registration";
            const string body = "Thanks for choosing Book The Room! Hope you will be satisfied with our service!";

            _emailService.SendEmail(newUser.Email, subject, body);

            return Ok();
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        [HttpGet("Profile/{userName}")]
        [Authorize]
        public async Task<IActionResult> Profile([FromRoute] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }

        //[HttpPut("Profile/{userName}/Edit")]
        //[Authorize]
        //public async Task<IActionResult> EditProfile([FromRoute] string userName, [FromBody] EditProfileViewModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(userName);

        //    if (user == null)
        //    {
        //        return NotFound(new { message = "User not found." });
        //    }

        //    // Здесь можно добавить логику для изменения профиля
        //    // Например, обновление Email, PhoneNumber и т.д.
        //    user.Email = model.Email;
        //    user.PhoneNumber = model.PhoneNumber;

        //    var result = await _userManager.UpdateAsync(user);

        //    if (result.Succeeded)
        //    {
        //        return Ok(new { message = "Profile updated successfully." });
        //    }

        //    return BadRequest(result.Errors);
        //}
    }
}
