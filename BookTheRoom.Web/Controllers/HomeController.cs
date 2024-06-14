using BookTheRoom.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;

namespace BookTheRoom.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [EnableRateLimiting("fixed")]
        [AllowAnonymous]
        public IActionResult Index()
        {           
            return View();
        }
        [AllowAnonymous]
        public IActionResult FAQ()
        {
            return View();
        }

        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Register()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Support()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
