using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.Product;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for product management service
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get paginated list of products
        /// </summary>
        Task<Models.PaginatedResponse<ProductViewModel>?> GetProductsAsync(int pageIndex, int pageSize, int? categoryId = null, string? searchTerm = null);

        /// <summary>
        /// Get all products
        /// </summary>
        Task<List<ProductViewModel>?> GetAllProductsAsync();

        /// <summary>
        /// Get product by ID
        /// </summary>
        Task<ProductViewModel?> GetProductByIdAsync(int id);

        /// <summary>
        /// Get products by category
        /// </summary>
        Task<List<ProductViewModel>?> GetProductsByCategoryAsync(int categoryId);

        /// <summary>
        /// Create new product
        /// </summary>
        Task<ProductViewModel?> CreateProductAsync(CreateProductViewModel request);

        /// <summary>
        /// Update product
        /// </summary>
        Task<ProductViewModel?> UpdateProductAsync(int id, UpdateProductViewModel request);

        /// <summary>
        /// Delete product
        /// </summary>
        Task<bool> DeleteProductAsync(int id);

        /// <summary>
        /// Update product stock
        /// </summary>
        Task<bool> UpdateStockAsync(int id, int quantity);

        /// <summary>
        /// Search products
        /// </summary>
        Task<List<ProductViewModel>?> SearchProductsAsync(string searchTerm);
    }

    /// <summary>
    /// Implementation of product service
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IApiService apiService, ILogger<ProductService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<Models.PaginatedResponse<ProductViewModel>?> GetProductsAsync(int pageIndex, int pageSize, int? categoryId = null, string? searchTerm = null)
        {
            try
            {
                var endpoint = $"/products?pageIndex={pageIndex}&pageSize={pageSize}";
                if (categoryId.HasValue)
                    endpoint += $"&categoryId={categoryId}";
                if (!string.IsNullOrEmpty(searchTerm))
                    endpoint += $"&searchTerm={searchTerm}";

                return await _apiService.GetAsync<Models.PaginatedResponse<ProductViewModel>>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting products: {ex.Message}");
                return null;
            }
        }

        public async Task<List<ProductViewModel>?> GetAllProductsAsync()
        {
            try
            {
                return await _apiService.GetAsync<List<ProductViewModel>>(ApiEndpoints.Products.All);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all products: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductViewModel?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _apiService.GetAsync<ProductViewModel>($"/products/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting product {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<ProductViewModel>?> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                return await _apiService.GetAsync<List<ProductViewModel>>($"/products/category/{categoryId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting products by category {categoryId}: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductViewModel?> CreateProductAsync(CreateProductViewModel request)
        {
            try
            {
                return await _apiService.PostAsync<ProductViewModel>(ApiEndpoints.Products.Base, request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductViewModel?> UpdateProductAsync(int id, UpdateProductViewModel request)
        {
            try
            {
                return await _apiService.PutAsync<ProductViewModel>($"/products/{id}", request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                return await _apiService.DeleteAsync($"/products/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateStockAsync(int id, int quantity)
        {
            try
            {
                var result = await _apiService.PutAsync<object>($"/products/{id}/stock", new { quantity });
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating stock for product {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ProductViewModel>?> SearchProductsAsync(string searchTerm)
        {
            try
            {
                return await _apiService.GetAsync<List<ProductViewModel>>($"/products/search?term={searchTerm}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching products: {ex.Message}");
                return null;
            }
        }
    }
}
