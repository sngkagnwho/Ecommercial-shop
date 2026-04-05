using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models;
using mtkpm.Admin.Models.Category;
using mtkpm.Admin.Models.Product;
using mtkpm.Admin.Services;
using System.Linq;
using System.Text.Json;

namespace mtkpm.Admin.Features.Products.Controllers
{
    /// <summary>
    /// Products management controller
    /// </summary>
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<ProductsController> _logger;
        private readonly IConfiguration _configuration;

        public ProductsController(
            ITokenManager tokenManager,
            ILogger<ProductsController> logger,
            IConfiguration configuration)
        {
            _tokenManager = tokenManager;
            _logger = logger;
            _configuration = configuration;
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

        private async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                var response = await httpClient.GetAsync($"{apiUrl}/categories");
                
                if (!response.IsSuccessStatusCode)
                    return new List<CategoryViewModel>();

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("data", out JsonElement dataElement) && 
                        dataElement.ValueKind != JsonValueKind.Null)
                    {
                        return JsonSerializer.Deserialize<List<CategoryViewModel>>(
                            dataElement.GetRawText(), options) ?? new List<CategoryViewModel>();
                    }
                }
                
                return new List<CategoryViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting categories: {ex.Message}");
                return new List<CategoryViewModel>();
            }
        }

        /// <summary>
        /// Display paginated list of products
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? searchTerm = null, int? categoryId = null)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var endpoint = $"{apiUrl}/products?pageIndex={pageIndex}&pageSize={pageSize}";
                if (categoryId.HasValue)
                    endpoint += $"&categoryId={categoryId}";
                if (!string.IsNullOrEmpty(searchTerm))
                    endpoint += $"&searchTerm={searchTerm}";

                var response = await httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET products - Status: {response.StatusCode}");
                
                var products = new List<ProductViewModel>();
                var pagination = new PaginationModel { CurrentPage = pageIndex, PageSize = pageSize };
                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
                {
                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        
                        using (JsonDocument doc = JsonDocument.Parse(responseContent))
                        {
                            JsonElement root = doc.RootElement;
                            
                            // Check for items field (paginated response)
                            if (root.TryGetProperty("data", out JsonElement dataElement) && 
                                dataElement.ValueKind != JsonValueKind.Null)
                            {
                                if (dataElement.TryGetProperty("items", out JsonElement itemsElement) &&
                                    itemsElement.ValueKind == JsonValueKind.Array)
                                {
                                    var itemsJson = itemsElement.GetRawText();
                                    products = JsonSerializer.Deserialize<List<ProductViewModel>>(itemsJson, options) ?? new List<ProductViewModel>();
                                }
                                
                                // Extract pagination info from API response
                                if (dataElement.TryGetProperty("totalCount", out JsonElement totalElement))
                                {
                                    pagination.TotalItems = totalElement.GetInt32();
                                }
                                if (dataElement.TryGetProperty("pageIndex", out JsonElement pageIndexElement))
                                {
                                    pagination.CurrentPage = pageIndexElement.GetInt32();
                                }
                                if (dataElement.TryGetProperty("pageSize", out JsonElement pageSizeElement))
                                {
                                    pagination.PageSize = pageSizeElement.GetInt32();
                                }
                            }
                        }
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    }
                    
                    _logger.LogInformation($"Products loaded: {products.Count}, Total: {pagination.TotalItems}");
                }
                else
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                }
                
                // Sort products by ID ascending
                products = products.OrderBy(p => p.Id).ToList();
                
                // Get categories for filter dropdown
                ViewBag.Categories = await GetCategoriesAsync();
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SelectedCategoryId = categoryId;
                ViewBag.Pagination = pagination;

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading products from API: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading products";
                return View(new List<ProductViewModel>());
            }
        }

        /// <summary>
        /// Display create product form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await GetCategoriesAsync();
            return View(new CreateProductViewModel());
        }

        /// <summary>
        /// Create new product
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await GetCategoriesAsync();
                return View(model);
            }

            try
            {
                // Ensure ImageUrl is null if empty (for optional field)
                if (string.IsNullOrWhiteSpace(model.ImageUrl))
                {
                    model.ImageUrl = null;
                }

                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(model),
                    System.Text.Encoding.UTF8,
                    "application/json");
                
                var response = await httpClient.PostAsync($"{apiUrl}/products", jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"POST products - Status: {response.StatusCode}");
                _logger.LogInformation($"Request: Name={model.Name}, Category={model.CategoryId}, Price={model.Price}, Stock={model.StockQuantity}");
                _logger.LogInformation($"Response: {responseContent}");
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Product created successfully";
                    return RedirectToAction(nameof(Index));
                }

                // Try to extract error message from response
                var errorMessage = $"Failed to create product (Status: {response.StatusCode})";
                if (!string.IsNullOrEmpty(responseContent))
                {
                    try
                    {
                        using (JsonDocument doc = JsonDocument.Parse(responseContent))
                        {
                            JsonElement root = doc.RootElement;
                            if (root.TryGetProperty("message", out JsonElement messageElement))
                            {
                                errorMessage = messageElement.GetString() ?? errorMessage;
                            }
                            // Also check for errors field
                            if (root.TryGetProperty("errors", out JsonElement errorsElement) &&
                                errorsElement.ValueKind == JsonValueKind.Array)
                            {
                                var errorsArray = errorsElement.EnumerateArray().ToList();
                                if (errorsArray.Count > 0)
                                {
                                    errorMessage += " - ";
                                    errorMessage += string.Join(", ", 
                                        errorsArray.Select(e => e.GetString()).Where(s => !string.IsNullOrEmpty(s)));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Error parsing create response: {ex.Message}");
                    }
                }

                _logger.LogError($"Create product failed: {errorMessage}");
                ViewBag.Categories = await GetCategoriesAsync();
                ViewBag.ErrorMessage = errorMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                ViewBag.Categories = await GetCategoriesAsync();
                ViewBag.ErrorMessage = "Error creating product: " + ex.Message;
                return View(model);
            }
        }

        /// <summary>
        /// Display product details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.GetAsync($"{apiUrl}/products/{id}");
                
                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                ProductViewModel? product = null;
                
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("data", out JsonElement dataElement) && 
                        dataElement.ValueKind != JsonValueKind.Null)
                    {
                        product = JsonSerializer.Deserialize<ProductViewModel>(
                            dataElement.GetRawText(), options);
                    }
                }
                
                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading product details: {ex.Message}");
                return NotFound();
            }
        }

        /// <summary>
        /// Display edit product form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.GetAsync($"{apiUrl}/products/{id}");
                
                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                ProductViewModel? product = null;
                
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("data", out JsonElement dataElement) && 
                        dataElement.ValueKind != JsonValueKind.Null)
                    {
                        product = JsonSerializer.Deserialize<ProductViewModel>(
                            dataElement.GetRawText(), options);
                    }
                }
                
                if (product == null)
                    return NotFound();

                var model = new UpdateProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    CategoryId = product.CategoryId
                };

                ViewBag.Categories = await GetCategoriesAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading product edit form: {ex.Message}");
                return NotFound();
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await GetCategoriesAsync();
                return View(model);
            }

            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(model),
                    System.Text.Encoding.UTF8,
                    "application/json");
                
                var response = await httpClient.PutAsync($"{apiUrl}/products/{id}", jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"PUT products/{id} - Status: {response.StatusCode}");
                _logger.LogInformation($"Response: {responseContent}");
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Product updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Categories = await GetCategoriesAsync();
                ViewBag.ErrorMessage = "Failed to update product";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product: {ex.Message}");
                ViewBag.Categories = await GetCategoriesAsync();
                ViewBag.ErrorMessage = "Error updating product";
                return View(model);
            }
        }

        /// <summary>
        /// Delete product with error message handling
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.DeleteAsync($"{apiUrl}/products/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"DELETE products/{id} - Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Product deleted successfully";
                    return RedirectToAction(nameof(Index));
                }

                // Try to extract error message from response
                var errorMessage = "Failed to delete product";
                if (!string.IsNullOrEmpty(responseContent))
                {
                    try
                    {
                        using (JsonDocument doc = JsonDocument.Parse(responseContent))
                        {
                            JsonElement root = doc.RootElement;
                            if (root.TryGetProperty("message", out JsonElement messageElement))
                            {
                                errorMessage = messageElement.GetString() ?? errorMessage;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Error parsing delete response: {ex.Message}");
                    }
                }

                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting product";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
