using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Auth;
using mtkpm.Admin.Services;
using System.Security.Claims;

namespace mtkpm.Admin.Features.Auth.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ITokenManager tokenManager, ILogger<AuthController> logger)
        {
            _authService = authService;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // If already logged in, redirect to dashboard
            if (_authService.IsAuthenticated())
                return RedirectToAction("Index", "Dashboard");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and password are required");
                return View();
            }

            var loginResult = await _authService.LoginAsync(username, password);
            if (loginResult == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            // Verify user is admin
            if (!loginResult.Roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Access denied. Admin role required.");
                return View();
            }

            // Create claims for cookie authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResult.UserId.ToString()),
                new Claim(ClaimTypes.Name, loginResult.UserName),
                new Claim(ClaimTypes.Email, loginResult.Email),
                new Claim("AccessToken", loginResult.AccessToken),
                new Claim("RefreshToken", loginResult.RefreshToken),
            };

            foreach (var role in loginResult.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = System.DateTimeOffset.UtcNow.AddHours(24)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation($"Admin user {username} logged in successfully");

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            _logger.LogInformation("Admin user logged out");
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Dashboard");
        }

        [NonAction]
        public new IActionResult Unauthorized()
        {
            return View();
        }
    }
}
