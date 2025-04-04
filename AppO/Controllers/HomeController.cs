using AppO.Models;
using AppO.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<appUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<appUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult Index(string? returnUrl)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Feed");
            }
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        public IActionResult Privacy()
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
