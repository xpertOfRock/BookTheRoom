using Api.Contracts.Account;
using Application.Interfaces;
using Core.Entities;
using Core.ValueObjects;
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





        [HttpPost]
        public async Task<IActionResult> Authorize(AuthorizeRequest request)
        {
            var thisUser = await _userManager.FindByEmailAsync(request.EmailOrUsername)
                ?? await _userManager.FindByNameAsync(request.EmailOrUsername);

            if (thisUser == null)
            {
                return Unauthorized();
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(thisUser, request.Password);

            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(thisUser, request.Password, false, false);

                if (result.Succeeded)
                {
                    return Redirect("/");
                }
            }
            else
            {
                Unauthorized();
            }
            return Ok();
        }
        
        [HttpPost]        
        public async Task<IActionResult> Register(RegisterRequest request)
        {

            var user = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                return Unauthorized();
            }

            var newUser = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Username,
                PhoneNumber = request.PhoneNumber,
                Age = request.Age,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Orders = new List<Order>(),
                Apartments = new List<Apartment>(),
                Comments = new List<Comment>()
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, request.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRole.Admin);
                await _signInManager.PasswordSignInAsync(newUser, request.Password, false, false);
            }

            const string subject = "Registration";
            const string body = "Thanks for choosing Book The Room! Hope you will be satisfied with our service!";

            _emailService.SendEmail(newUser.Email, subject, body);
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete("access_token");
            return Redirect("/");
        }

        [Authorize]
        [HttpGet("{id}")]      
        public async Task<IActionResult> Profile([FromRoute] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return Ok(user);
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProfile([FromRoute] string userName)
        {
            return Ok();
        }

    }
}
