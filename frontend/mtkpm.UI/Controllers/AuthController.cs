using Microsoft.AspNetCore.Mvc;

namespace mtkpm.UI.Controllers
{
    public class AuthController : Controller
    {
        // GET /Auth/Login
        public IActionResult Index()
        {
            return View();
        }

        // GET /Auth/Register
        public IActionResult Register()
        {
            return View("Login"); // Reuse the same view; JS handles tab switching
        }
    }
}
