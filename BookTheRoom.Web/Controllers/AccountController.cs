using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.Infrastructure.Data.Interfaces;
using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.Web.Interfaces;
using BookTheRoom.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookTheRoom.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailService emailService,
                                 ITokenService tokenService)
        {
            _tokenService = tokenService;
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
                    return RedirectToAction("Index", "Home");
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
                Address = new Address()
                {
                    Country = registerViewModel.Address.Country,
                    City = registerViewModel.Address.City,
                    StreetOrDistrict = registerViewModel.Address.StreetOrDistrict,
                    Index = registerViewModel.Address.Index
                }
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRole.User);
            }

            const string subject = "Registration";
            const string body = "Thanks for choosing Book The Room! Hope you will be satisfied with our service!";

            _emailService.SendEmail(newUser.Email, subject, body);
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [Route("/Profile/{userName}", Name = "Profile")]
        public async Task<IActionResult> Profile([FromRoute] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return View(user);
        }


        [Authorize]
        [Route("/Profile/{userName}/Edit", Name = "EditProfile")]
        public async Task<IActionResult> EditProfile([FromRoute] string userName)
        {
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
