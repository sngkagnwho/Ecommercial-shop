using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Services;
using mtkpm.Admin.Models.Notification;

namespace mtkpm.Admin.Features.Notifications.Controllers
{
    /// <summary>
    /// Notification Management Controller
    /// Manages notification method subscriptions and tests notification events
    /// </summary>
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly IAdminNotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(IAdminNotificationService notificationService, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Display notification methods management dashboard
        /// Shows available notification methods and their active status
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var methods = await _notificationService.GetNotificationMethodsAsync();
                var subscribers = await _notificationService.GetSubscribersAsync();

                ViewBag.Methods = methods ?? new List<NotificationMethodViewModel>();
                ViewBag.Subscribers = subscribers;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading notification methods: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading notification methods";
                return View();
            }
        }

        /// <summary>
        /// Subscribe to notification method
        /// POST /Notifications/Subscribe
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string methodName)
        {
            try
            {
                var request = new NotificationSubscriptionRequest { Email = "", SubscribeToAll = true };
                var success = await _notificationService.SubscribeNotificationMethodAsync(methodName, request);
                if (success)
                {
                    TempData["SuccessMessage"] = $"✅ Successfully subscribed to {methodName}";
                }
                else
                {
                    TempData["ErrorMessage"] = $"❌ Failed to subscribe to {methodName}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error subscribing to {methodName}: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Unsubscribe from notification method
        /// POST /Notifications/Unsubscribe
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(string methodName)
        {
            try
            {
                var success = await _notificationService.UnsubscribeNotificationMethodAsync(methodName);
                if (success)
                {
                    TempData["SuccessMessage"] = $"✅ Successfully unsubscribed from {methodName}";
                }
                else
                {
                    TempData["ErrorMessage"] = $"❌ Failed to unsubscribe from {methodName}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error unsubscribing from {methodName}: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Test Order Created event
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestOrderCreated()
        {
            try
            {
                var result = await _notificationService.TestOrderCreatedEventAsync();
                if (result != null)
                {
                    TempData["TestMessage"] = "✅ Order Created event test sent to all subscribers";
                }
                else
                {
                    TempData["TestErrorMessage"] = "❌ Failed to test Order Created event";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing Order Created event: {ex.Message}");
                TempData["TestErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Test Payment Completed event
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestPaymentCompleted()
        {
            try
            {
                var result = await _notificationService.TestPaymentCompletedEventAsync();
                if (result != null)
                {
                    TempData["TestMessage"] = "✅ Payment Completed event test sent to all subscribers";
                }
                else
                {
                    TempData["TestErrorMessage"] = "❌ Failed to test Payment Completed event";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing Payment Completed event: {ex.Message}");
                TempData["TestErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Test Order Shipped event
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestOrderShipped()
        {
            try
            {
                var result = await _notificationService.TestOrderShippedEventAsync();
                if (result != null)
                {
                    TempData["TestMessage"] = "✅ Order Shipped event test sent to all subscribers";
                }
                else
                {
                    TempData["TestErrorMessage"] = "❌ Failed to test Order Shipped event";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing Order Shipped event: {ex.Message}");
                TempData["TestErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Test Payment Failed event
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestPaymentFailed()
        {
            try
            {
                var result = await _notificationService.TestPaymentFailedEventAsync();
                if (result != null)
                {
                    TempData["TestMessage"] = "✅ Payment Failed event test sent to all subscribers";
                }
                else
                {
                    TempData["TestErrorMessage"] = "❌ Failed to test Payment Failed event";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing Payment Failed event: {ex.Message}");
                TempData["TestErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Test Order Cancelled event
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestOrderCancelled()
        {
            try
            {
                var result = await _notificationService.TestOrderCancelledEventAsync();
                if (result != null)
                {
                    TempData["TestMessage"] = "✅ Order Cancelled event test sent to all subscribers";
                }
                else
                {
                    TempData["TestErrorMessage"] = "❌ Failed to test Order Cancelled event";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing Order Cancelled event: {ex.Message}");
                TempData["TestErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
