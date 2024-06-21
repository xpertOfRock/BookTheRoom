using BookTheRoom.Application.Interfaces;
using BookTheRoom.Core.ValueObjects;
using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookTheRoom.WebUI.Controllers
{
    public class AccountController : Controller
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


        [Route("/Login", Name = "Login")]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }


        [HttpPost]
        [Route("/Login", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid) { return View(login); }

            ApplicationUser thisUser;

            var userByEmail = await _userManager.FindByEmailAsync(login.EmailOrUsername);
            var userByName = await _userManager.FindByNameAsync(login.EmailOrUsername);
            
            if (userByEmail != null && login.EmailOrUsername == userByEmail.Email)
            {
                thisUser = userByEmail;
            }
            else
            {
                thisUser = userByName;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(thisUser, login.Password);

            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(thisUser, login.Password, false, false);

                if (result.Succeeded)
                {                   
                    return Redirect("/");
                }
            }
            else
            {
                TempData["Error"] = "Wrong password. Try again.";
                return View(login);
            }
            TempData["Error"] = "Wrong username or email address. Try again.";
            return View(login);
        }


        [Route("/Register", Name = "Register")]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }


        [HttpPost]
        [Route("/Register", Name = "Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) { return View(registerViewModel); }

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            user = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (user != null)
            {
                TempData["Error"] = "This username is already in use";
                return View(registerViewModel);
            }
            var newUser = new ApplicationUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.UserName,
                PhoneNumber = registerViewModel.PhoneNumber,               
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRole.Admin);
                await _signInManager.PasswordSignInAsync(newUser, registerViewModel.Password, false, false);
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
        [Route("/Profile/{userName}", Name = "Profile")]
        public async Task<IActionResult> Profile([FromRoute] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return View(user);
        }


        
        [Route("/Profile/{userName}/Edit", Name = "EditProfile")]
        public async Task<IActionResult> EditProfile([FromRoute] string userName)
        {
            return View();
        }
               
    }
}
