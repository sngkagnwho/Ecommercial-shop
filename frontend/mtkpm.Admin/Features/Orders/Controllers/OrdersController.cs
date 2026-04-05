using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Features.Orders.Models;
using mtkpm.Admin.Models;
using mtkpm.Admin.Services;
using System.Text.Json;

namespace mtkpm.Admin.Features.Orders.Controllers
{
    /// <summary>
    /// Orders management controller
    /// </summary>
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<OrdersController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Services.IUserAddressService _userAddressService;

        public OrdersController(
            ITokenManager tokenManager,
            ILogger<OrdersController> logger,
            IConfiguration configuration,
            Services.IUserAddressService userAddressService)
        {
            _tokenManager = tokenManager;
            _logger = logger;
            _configuration = configuration;
            _userAddressService = userAddressService;
        }

        private string GetApiBaseUrl()
        {
            return _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
        }

        private HttpClient GetHttpClientWithAuth()
        {
            var httpClient = new HttpClient();
            var token = _tokenManager.GetToken();
            
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation($"Authorization header added - Token length: {token.Length}");
            }
            else
            {
                _logger.LogWarning("No token available - Authorization header NOT added");
            }
            
            return httpClient;
        }

        /// <summary>
        /// Display paginated list of all orders
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.GetAsync($"{apiUrl}/orders");
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET orders - Status: {response.StatusCode}");
                
                var orders = new List<OrderViewModel>();
                var pagination = new PaginationModel { CurrentPage = pageIndex, PageSize = pageSize };
                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
                {
                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        
                        using (JsonDocument doc = JsonDocument.Parse(responseContent))
                        {
                            JsonElement root = doc.RootElement;
                            
                            if (root.TryGetProperty("data", out JsonElement dataElement) && 
                                dataElement.ValueKind == JsonValueKind.Array)
                            {
                                var itemsJson = dataElement.GetRawText();
                                var allOrders = JsonSerializer.Deserialize<List<OrderViewModel>>(itemsJson, options) ?? new List<OrderViewModel>();
                                
                                // Manual pagination
                                pagination.TotalItems = allOrders.Count;
                                orders = allOrders
                                    .OrderByDescending(o => o.OrderDate)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                            }
                        }
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    }
                    
                    _logger.LogInformation($"Orders loaded: {orders.Count}, Total: {pagination.TotalItems}");
                }
                else
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                }
                
                // Sort orders by ID ascending
                orders = orders.OrderBy(o => o.Id).ToList();
                
                ViewBag.Pagination = pagination;
                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading orders from API: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading orders";
                return View(new List<OrderViewModel>());
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
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.GetAsync($"{apiUrl}/orders/{id}");
                
                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                OrderViewModel? order = null;
                
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("data", out JsonElement dataElement) && 
                        dataElement.ValueKind != JsonValueKind.Null)
                    {
                        order = JsonSerializer.Deserialize<OrderViewModel>(
                            dataElement.GetRawText(), options);
                    }
                }
                
                if (order == null)
                    return NotFound();

                // Load user's saved addresses
                var userAddresses = await _userAddressService.GetMyAddressesAsync();
                ViewBag.UserAddresses = userAddresses;

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading order details: {ex.Message}");
                return NotFound();
            }
        }

        /// <summary>
        /// Update order status form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.GetAsync($"{apiUrl}/orders/{id}");
                
                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                OrderViewModel? order = null;
                
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("data", out JsonElement dataElement) && 
                        dataElement.ValueKind != JsonValueKind.Null)
                    {
                        order = JsonSerializer.Deserialize<OrderViewModel>(
                            dataElement.GetRawText(), options);
                    }
                }
                
                if (order == null)
                    return NotFound();

                ViewBag.CurrentStatus = order.Status;
                ViewBag.OrderId = id;
                return View(order);
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
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                ModelState.AddModelError("", "Status is required");
                ViewBag.OrderId = id;
                return View();
            }

            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                // Parse status as integer
                if (!int.TryParse(status, out var statusInt))
                {
                    ModelState.AddModelError("", "Invalid status value");
                    ViewBag.OrderId = id;
                    return View();
                }

                var updateDto = new { status = statusInt };
                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(updateDto),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PatchAsync($"{apiUrl}/orders/{id}/status", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Order status updated successfully";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error updating order status: {errorContent}");
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
    }
}
