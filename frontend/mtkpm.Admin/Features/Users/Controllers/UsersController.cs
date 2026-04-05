using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.User;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Users.Controllers
{
    /// <summary>
    /// Users management controller
    /// </summary>
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Display paginated list of users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? searchTerm = null)
        {
            try
            {
                // Get user statistics
                var stats = await _userService.GetUserStatisticsAsync();
                ViewBag.Statistics = stats ?? new UserStatisticsViewModel();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var searchResults = await _userService.SearchUsersAsync(searchTerm);
                    ViewBag.SearchTerm = searchTerm;
                    return View(searchResults ?? new List<UserViewModel>());
                }

                var result = await _userService.GetUsersAsync(pageIndex, pageSize);
                ViewBag.SearchTerm = searchTerm;
                return View(result ?? new List<UserViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading users: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading users";
                return View(new List<UserViewModel>());
            }
        }

        /// <summary>
        /// Display user details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found or cannot be accessed";
                    return RedirectToAction(nameof(Index));
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading user details: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading user details";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Display create user form
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _userService.CreateUserAsync(model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "User created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to create user";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                ViewBag.ErrorMessage = "Error creating user";
                return View(model);
            }
        }

        /// <summary>
        /// Display edit user form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found or cannot be accessed";
                    return RedirectToAction(nameof(Index));
                }

                var model = new UpdateUserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading user edit form: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading user for editing";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateUserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _userService.UpdateUserAsync(id, model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "User updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to update user";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user: {ex.Message}");
                ViewBag.ErrorMessage = "Error updating user";
                return View(model);
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "User deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete user";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting user";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Update user role (Admin only)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(int userId, string roleName)
        {
            try
            {
                var success = await _userService.UpdateUserRoleAsync(userId, roleName);
                if (success)
                {
                    TempData["SuccessMessage"] = $"User role updated to {roleName}";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update user role";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user role: {ex.Message}");
                TempData["ErrorMessage"] = "Error updating user role";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Lock or unlock user account (Admin only)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(int userId, bool isLocked)
        {
            try
            {
                var success = await _userService.LockUserAsync(userId, isLocked);
                if (success)
                {
                    var action = isLocked ? "locked" : "unlocked";
                    TempData["SuccessMessage"] = $"User has been {action}";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to lock/unlock user";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error locking user: {ex.Message}");
                TempData["ErrorMessage"] = "Error locking/unlocking user";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Get user statistics for dashboard
        /// </summary>
        [HttpGet]
        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var stats = await _userService.GetUserStatisticsAsync();
                return PartialView("_Dashboard", stats ?? new UserStatisticsViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading user statistics: {ex.Message}");
                return PartialView("_Dashboard", new UserStatisticsViewModel());
            }
        }
    }
}
