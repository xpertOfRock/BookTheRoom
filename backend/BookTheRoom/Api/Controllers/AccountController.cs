using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public Task<IActionResult> Authorize()
        //{
        //    return Ok();
        //}
        //public Task<IActionResult> Register()
        //{
        //    return Ok();
        //}

        //public Task<IActionResult> Account()
        //{
        //    return Ok();
        //}

    }
}
