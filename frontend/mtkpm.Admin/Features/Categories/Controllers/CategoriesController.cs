using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Category;
using mtkpm.Admin.Services;
using System.Text.Json;

namespace mtkpm.Admin.Features.Categories.Controllers
{
    /// <summary>
    /// Categories management controller
    /// </summary>
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly BackendApiClient _apiClient;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IConfiguration _configuration;

        public CategoriesController(
            ICategoryService categoryService, 
            BackendApiClient apiClient, 
            ITokenManager tokenManager,
            ILogger<CategoriesController> logger, 
            IConfiguration configuration)
        {
            _categoryService = categoryService;
            _apiClient = apiClient;
            _tokenManager = tokenManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get raw response as string to avoid dynamic type issues
                var token = _tokenManager.GetToken();
                var httpClient = new HttpClient();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var apiUrl = _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
                var response = await httpClient.GetAsync($"{apiUrl}/categories");
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"API Response: {responseContent}");
                
                var categories = new List<CategoryViewModel>();
                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
                {
                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        
                        using (JsonDocument doc = JsonDocument.Parse(responseContent))
                        {
                            JsonElement root = doc.RootElement;
                            
                            // Check if it's a wrapper response with "data" field
                            if (root.ValueKind == JsonValueKind.Object)
                            {
                                if (root.TryGetProperty("data", out JsonElement dataElement) && 
                                    dataElement.ValueKind != JsonValueKind.Null)
                                {
                                    _logger.LogInformation($"Found data wrapper, deserializing...");
                                    var dataJson = dataElement.GetRawText();
                                    categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(
                                        dataJson, options) ?? new List<CategoryViewModel>();
                                }
                            }
                            else if (root.ValueKind == JsonValueKind.Array)
                            {
                                // Direct array response
                                _logger.LogInformation($"Direct array response, deserializing...");
                                categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(
                                    responseContent, options) ?? new List<CategoryViewModel>();
                            }
                        }
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    }
                    
                    _logger.LogInformation($"Categories loaded: {categories.Count}");
                }
                else
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                }

                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading categories from API: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                TempData["ErrorMessage"] = "Error loading categories";
                return View(new List<CategoryViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Use BackendApiClient for API call
                var result = await _apiClient.PostAsync<dynamic>("categories", model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Category created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to create category");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                ModelState.AddModelError("", "Error creating category");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var token = _tokenManager.GetToken();
                var httpClient = new HttpClient();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var apiUrl = _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
                var response = await httpClient.GetAsync($"{apiUrl}/categories/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"Details API Response: {responseContent}");
                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (JsonDocument doc = JsonDocument.Parse(responseContent))
                    {
                        JsonElement root = doc.RootElement;
                        
                        // Check if wrapped in ApiResponse
                        if (root.ValueKind == JsonValueKind.Object)
                        {
                            if (root.TryGetProperty("data", out JsonElement dataElement) && 
                                dataElement.ValueKind != JsonValueKind.Null)
                            {
                                var category = JsonSerializer.Deserialize<CategoryViewModel>(
                                    dataElement.GetRawText(), options);
                                if (category != null)
                                    return View(category);
                            }
                        }
                    }
                }
                
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading category details: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading category details";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = _tokenManager.GetToken();
                var httpClient = new HttpClient();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var apiUrl = _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
                var response = await httpClient.GetAsync($"{apiUrl}/categories/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"Edit Load API Response: {responseContent}");
                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (JsonDocument doc = JsonDocument.Parse(responseContent))
                    {
                        JsonElement root = doc.RootElement;
                        
                        // Check if wrapped in ApiResponse
                        if (root.ValueKind == JsonValueKind.Object)
                        {
                            if (root.TryGetProperty("data", out JsonElement dataElement) && 
                                dataElement.ValueKind != JsonValueKind.Null)
                            {
                                var category = JsonSerializer.Deserialize<CategoryViewModel>(
                                    dataElement.GetRawText(), options);
                                if (category != null)
                                    return View(category);
                            }
                        }
                    }
                }
                
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading category for edit: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading category";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload category data for display
                var category = await GetCategoryForEdit(id);
                if (category != null)
                    return View(category);
                return NotFound();
            }

            try
            {
                // Ensure ID matches
                model.Id = id;
                
                // Call backend API with manual token injection (same as Details)
                var token = _tokenManager.GetToken();
                var httpClient = new HttpClient();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var apiUrl = _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
                var json = System.Text.Json.JsonSerializer.Serialize(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await httpClient.PutAsync($"{apiUrl}/categories/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"Edit API Response ({response.StatusCode}): {responseContent}");
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Category updated successfully";
                    return RedirectToAction(nameof(Details), new { id });
                }
                else
                {
                    _logger.LogError($"API update failed: {response.StatusCode} - {responseContent}");
                    ModelState.AddModelError("", $"Failed to update category: {response.StatusCode}");
                    
                    // Reload category for display
                    var category = await GetCategoryForEdit(id);
                    if (category != null)
                        return View(category);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category: {ex.Message}");
                ModelState.AddModelError("", "Error updating category");
                
                // Reload category for display
                var category = await GetCategoryForEdit(id);
                if (category != null)
                    return View(category);
                return NotFound();
            }
        }

        private async Task<CategoryViewModel> GetCategoryForEdit(int id)
        {
            try
            {
                var token = _tokenManager.GetToken();
                var httpClient = new HttpClient();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var apiUrl = _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
                var response = await httpClient.GetAsync($"{apiUrl}/categories/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        JsonElement root = doc.RootElement;
                        
                        if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            if (root.TryGetProperty("data", out JsonElement dataElement) && 
                                dataElement.ValueKind != System.Text.Json.JsonValueKind.Null)
                            {
                                return System.Text.Json.JsonSerializer.Deserialize<CategoryViewModel>(
                                    dataElement.GetRawText(), options);
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading category: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var (success, errorMessage) = await _apiClient.DeleteWithErrorAsync($"categories/{id}");
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Category deleted successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = string.IsNullOrEmpty(errorMessage) ? "Failed to delete category" : errorMessage;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting category";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
