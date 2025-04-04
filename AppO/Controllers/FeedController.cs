using Microsoft.AspNetCore.Mvc;

namespace AppO.Controllers
{
    public class FeedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
