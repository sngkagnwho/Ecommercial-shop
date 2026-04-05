using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Notification;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Notifications.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsAsync();
                return View(notifications ?? new List<NotificationViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading notifications: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading notifications";
                return View(new List<NotificationViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Send()
        {
            return View(new SendNotificationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendNotificationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var success = await _notificationService.SendNotificationAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Notification sent successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to send notification";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending notification: {ex.Message}");
                ViewBag.ErrorMessage = "Error sending notification";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _notificationService.DeleteNotificationAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Notification deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete notification";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting notification: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting notification";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
