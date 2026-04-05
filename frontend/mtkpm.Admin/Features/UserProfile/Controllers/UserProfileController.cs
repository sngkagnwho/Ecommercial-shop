using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.User;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.UserProfile.Controllers
{
    /// <summary>
    /// Controller for managing current user profile
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IUserService userService, ILogger<UserProfileController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Display current user profile information
        /// </summary>
        [HttpGet]
        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Loading current user profile");
                
                var user = await _userService.GetCurrentUserAsync();
                
                if (user == null)
                {
                    _logger.LogWarning("Failed to load current user profile");
                    TempData["ErrorMessage"] = "Unable to load your profile information";
                    return RedirectToAction("Index", "Home");
                }

                return View("Index", user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading user profile: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading your profile";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Display edit current user profile form
        /// </summary>
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit()
        {
            try
            {
                _logger.LogInformation("Loading edit profile form");
                
                var user = await _userService.GetCurrentUserAsync();
                
                if (user == null)
                {
                    _logger.LogWarning("Failed to load current user for editing");
                    TempData["ErrorMessage"] = "Unable to load your profile information";
                    return RedirectToAction("Index");
                }

                var model = new UpdateCurrentUserViewModel
                {
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return View("Edit", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading edit profile form: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading the edit form";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Update current user profile
        /// </summary>
        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCurrentUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for profile update");
                    return View("Edit", model);
                }

                _logger.LogInformation("Updating current user profile");
                
                var result = await _userService.UpdateCurrentUserAsync(model);
                
                if (result == null)
                {
                    _logger.LogWarning("Failed to update current user profile");
                    ModelState.AddModelError(string.Empty, "Failed to update your profile");
                    return View("Edit", model);
                }

                _logger.LogInformation($"User profile updated successfully: {result.Id}");
                TempData["SuccessMessage"] = "Your profile has been updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user profile: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating your profile");
                return View("Edit", model);
            }
        }
    }
}
