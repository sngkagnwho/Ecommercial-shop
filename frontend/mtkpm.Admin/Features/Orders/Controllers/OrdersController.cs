using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Order;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Orders.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IAdminOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IAdminOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        // GET: /Orders
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "", int? status = null, int page = 1)
        {
            try
            {
                var statistics = await _orderService.GetOrderStatisticsAsync();
                var allOrders = await _orderService.GetAllOrdersAsync();

                // Filter by status if provided
                if (status.HasValue)
                {
                    allOrders = allOrders.Where(o => o.Status == status.Value).ToList();
                }

                // Filter by search term (order number or customer name)
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    allOrders = allOrders.Where(o =>
                        o.OrderNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        (o.UserName != null && o.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Convert to list view models
                var orderListModels = allOrders.Select(o => new OrderListViewModel
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    CustomerName = o.UserName,
                    TotalItems = o.OrderItems?.Count ?? 0,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    IsPaid = o.IsPaid,
                    PaymentMethodDisplay = ((PaymentMethodType)o.PaymentMethod).ToString()
                }).OrderByDescending(o => o.OrderDate).ToList();

                ViewBag.Statistics = statistics;
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SelectedStatus = status;
                ViewBag.StatusList = GetStatusList();

                return View(orderListModels);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Index", "Orders") });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading orders: {ex.Message}");
                ViewBag.ErrorMessage = $"Error loading orders: {ex.Message}";
                return View(new List<OrderListViewModel>());
            }
        }

        // GET: /Orders/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found";
                    return RedirectToAction("Index");
                }

                // Convert to detail view model
                var detailModel = new OrderDetailViewModel
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    UserId = order.UserId,
                    UserName = order.UserName,
                    ShippingAddress = order.ShippingAddress,
                    BillingAddress = order.BillingAddress,
                    SubTotal = order.SubTotal,
                    ShippingFee = order.ShippingFee,
                    Discount = order.Discount,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    PaymentMethod = order.PaymentMethod,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    Note = order.Note,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderItems = order.OrderItems?.Select(i => new OrderItemDetailViewModel
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        PriceAtOrder = i.PriceAtOrder,
                        TotalPrice = i.TotalPrice
                    }).ToList() ?? new List<OrderItemDetailViewModel>()
                };

                ViewBag.StatusList = GetStatusList();

                return View(detailModel);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Details", "Orders", new { id }) });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading order {id}: {ex.Message}");
                TempData["ErrorMessage"] = $"Error loading order: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /Orders/UpdateStatus/5
        [HttpPost("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            try
            {
                var result = await _orderService.UpdateOrderStatusAsync(id, status);

                if (result != null)
                {
                    TempData["SuccessMessage"] = $"Order status updated to {((OrderStatus)status).ToString()}";
                    return RedirectToAction("Details", new { id });
                }

                TempData["ErrorMessage"] = "Failed to update order status";
                return RedirectToAction("Details", new { id });
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Details", "Orders", new { id }) });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order status: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Details", new { id });
            }
        }

        // POST: /Orders/MarkAsPaid/5
        [HttpPost("MarkAsPaid/{id}")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            try
            {
                var result = await _orderService.MarkOrderAsPaidAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = "Order marked as paid";
                    return RedirectToAction("Details", new { id });
                }

                TempData["ErrorMessage"] = "Failed to mark order as paid";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking order as paid: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Details", new { id });
            }
        }

        // POST: /Orders/Cancel/5
        [HttpPost("Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found";
                    return RedirectToAction("Index");
                }

                // Check if order can be cancelled
                if (order.Status == (int)OrderStatus.Shipping || 
                    order.Status == (int)OrderStatus.Delivered || 
                    order.Status == (int)OrderStatus.Completed)
                {
                    TempData["ErrorMessage"] = $"Cannot cancel order in {((OrderStatus)order.Status).ToString()} status";
                    return RedirectToAction("Details", new { id });
                }

                var result = await _orderService.CancelOrderAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = "Order cancelled successfully";
                    return RedirectToAction("Details", new { id });
                }

                TempData["ErrorMessage"] = "Failed to cancel order";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error cancelling order: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Details", new { id });
            }
        }

        // Helper method to get status list for dropdown
        private List<StatusItem> GetStatusList()
        {
            return new List<StatusItem>
            {
                new StatusItem { Value = (int)OrderStatus.Pending, Name = OrderStatus.Pending.ToString(), Badge = "badge-warning" },
                new StatusItem { Value = (int)OrderStatus.Confirmed, Name = OrderStatus.Confirmed.ToString(), Badge = "badge-info" },
                new StatusItem { Value = (int)OrderStatus.Processing, Name = OrderStatus.Processing.ToString(), Badge = "badge-primary" },
                new StatusItem { Value = (int)OrderStatus.Shipping, Name = OrderStatus.Shipping.ToString(), Badge = "badge-secondary" },
                new StatusItem { Value = (int)OrderStatus.Delivered, Name = OrderStatus.Delivered.ToString(), Badge = "badge-success" },
                new StatusItem { Value = (int)OrderStatus.Completed, Name = OrderStatus.Completed.ToString(), Badge = "badge-success" },
                new StatusItem { Value = (int)OrderStatus.Cancelled, Name = OrderStatus.Cancelled.ToString(), Badge = "badge-danger" },
                new StatusItem { Value = (int)OrderStatus.Returned, Name = OrderStatus.Returned.ToString(), Badge = "badge-orange" },
                new StatusItem { Value = (int)OrderStatus.Failed, Name = OrderStatus.Failed.ToString(), Badge = "badge-dark" }
            };
        }
    }
}
