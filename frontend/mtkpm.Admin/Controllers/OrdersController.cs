using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Controllers
{
    /// <summary>
    /// Orders management controller
    /// </summary>
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Display paginated list of orders
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var result = await _orderService.GetOrdersAsync(pageIndex, pageSize);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading orders: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading orders";
                return View(new Models.PaginatedResponse<Models.Order.OrderViewModel>());
            }
        }

        /// <summary>
        /// Display order details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound();

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading order details: {ex.Message}");
                return NotFound();
            }
        }

        /// <summary>
        /// Display update order status form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound();

                ViewBag.CurrentStatus = order.Status;
                ViewBag.OrderId = id;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading update status form: {ex.Message}");
                return NotFound();
            }
        }

        /// <summary>
        /// Update order status
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status, string? note = null)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                ModelState.AddModelError("", "Status is required");
                ViewBag.OrderId = id;
                return View();
            }

            try
            {
                var success = await _orderService.UpdateOrderStatusAsync(id, status, note);
                if (success)
                {
                    TempData["SuccessMessage"] = "Order status updated successfully";
                    return RedirectToAction(nameof(Details), new { id });
                }

                ViewBag.ErrorMessage = "Failed to update order status";
                ViewBag.OrderId = id;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order status: {ex.Message}");
                ViewBag.ErrorMessage = "Error updating order status";
                ViewBag.OrderId = id;
                return View();
            }
        }

        /// <summary>
        /// Mark order as paid
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            try
            {
                var success = await _orderService.MarkOrderAsPaidAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Order marked as paid";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to mark order as paid";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking order as paid: {ex.Message}");
                TempData["ErrorMessage"] = "Error marking order as paid";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
