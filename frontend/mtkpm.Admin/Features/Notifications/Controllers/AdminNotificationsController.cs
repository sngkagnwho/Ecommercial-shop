using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Notification;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Notifications.Controllers
{
    [Authorize]
    public class AdminNotificationsController : Controller
    {
        private readonly IAdminNotificationService _notificationService;
        private readonly ILogger<AdminNotificationsController> _logger;

        public AdminNotificationsController(
            IAdminNotificationService notificationService,
            ILogger<AdminNotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Display notification console dashboard with methods, subscribers, and statistics
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var methods = await _notificationService.GetNotificationMethodsAsync();
                var subscribers = await _notificationService.GetSubscribersAsync();

                // Build statistics
                var stats = new NotificationStatisticsViewModel
                {
                    TotalMethods = methods?.Count ?? 0,
                    ActiveMethods = methods?.Count(m => m.IsActive) ?? 0,
                    TotalSubscribers = subscribers?.Count ?? 0,
                    NotificationsSentToday = subscribers?.Sum(s => s.TotalNotificationsSent) ?? 0,
                    NotificationsFailedToday = subscribers?.Sum(s => s.FailedNotifications) ?? 0,
                    EventStatistics = new List<NotificationEventStatistic>
                    {
                        new NotificationEventStatistic { EventName = "OrderCreated", TotalSent = 0, Failed = 0 },
                        new NotificationEventStatistic { EventName = "PaymentCompleted", TotalSent = 0, Failed = 0 },
                        new NotificationEventStatistic { EventName = "OrderShipped", TotalSent = 0, Failed = 0 },
                        new NotificationEventStatistic { EventName = "PaymentFailed", TotalSent = 0, Failed = 0 },
                        new NotificationEventStatistic { EventName = "OrderCancelled", TotalSent = 0, Failed = 0 }
                    }
                };

                ViewBag.Statistics = stats;
                ViewBag.Methods = methods ?? new List<NotificationMethodViewModel>();
                ViewBag.Subscribers = subscribers ?? new List<NotificationSubscriberViewModel>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading notification console: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading notification console";
                ViewBag.Statistics = new NotificationStatisticsViewModel();
                ViewBag.Methods = new List<NotificationMethodViewModel>();
                ViewBag.Subscribers = new List<NotificationSubscriberViewModel>();
                return View();
            }
        }

        /// <summary>
        /// Test OrderCreated event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TestOrderCreatedEvent()
        {
            try
            {
                var result = await _notificationService.TestOrderCreatedEventAsync();
                TempData["TestEvent"] = $"OrderCreated: {result?.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing OrderCreated event: {ex.Message}");
                TempData["ErrorMessage"] = "Error testing OrderCreated event";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Test PaymentCompleted event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TestPaymentCompletedEvent()
        {
            try
            {
                var result = await _notificationService.TestPaymentCompletedEventAsync();
                TempData["TestEvent"] = $"PaymentCompleted: {result?.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing PaymentCompleted event: {ex.Message}");
                TempData["ErrorMessage"] = "Error testing PaymentCompleted event";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Test OrderShipped event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TestOrderShippedEvent()
        {
            try
            {
                var result = await _notificationService.TestOrderShippedEventAsync();
                TempData["TestEvent"] = $"OrderShipped: {result?.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing OrderShipped event: {ex.Message}");
                TempData["ErrorMessage"] = "Error testing OrderShipped event";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Test PaymentFailed event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TestPaymentFailedEvent()
        {
            try
            {
                var result = await _notificationService.TestPaymentFailedEventAsync();
                TempData["TestEvent"] = $"PaymentFailed: {result?.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing PaymentFailed event: {ex.Message}");
                TempData["ErrorMessage"] = "Error testing PaymentFailed event";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Test OrderCancelled event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TestOrderCancelledEvent()
        {
            try
            {
                var result = await _notificationService.TestOrderCancelledEventAsync();
                TempData["TestEvent"] = $"OrderCancelled: {result?.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing OrderCancelled event: {ex.Message}");
                TempData["ErrorMessage"] = "Error testing OrderCancelled event";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
