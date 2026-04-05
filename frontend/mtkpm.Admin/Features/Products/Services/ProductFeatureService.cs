using mtkpm.Admin.Features.Products.Models;
using mtkpm.Admin.Infrastructure.Http;
using mtkpm.Admin.Infrastructure.Caching;

namespace mtkpm.Admin.Features.Products.Services
{
    /// <summary>
    /// Implementation of product feature service
    /// </summary>
    public class ProductFeatureService : IProductFeatureService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ICacheService _cache;
        private readonly ILogger<ProductFeatureService> _logger;

        public ProductFeatureService(IHttpClientWrapper httpClient, ICacheService cache, ILogger<ProductFeatureService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<(List<ProductDto> Items, int TotalCount)?> GetProductsAsync(int pageIndex, int pageSize, string? searchTerm = null, int? categoryId = null)
        {
            try
            {
                var endpoint = $"products?pageIndex={pageIndex}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(searchTerm))
                    endpoint += $"&searchTerm={searchTerm}";
                if (categoryId.HasValue)
                    endpoint += $"&categoryId={categoryId}";

                var products = await _httpClient.GetAsync<List<ProductDto>>(endpoint);
                return products != null ? (products, products.Count) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting products: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductDetailDto?> GetProductByIdAsync(int id)
        {
            try
            {
                var cacheKey = $"product_{id}";
                var cached = _cache.Get<ProductDetailDto>(cacheKey);
                if (cached != null)
                    return cached;

                var product = await _httpClient.GetAsync<ProductDetailDto>($"products/{id}");
                if (product != null)
                {
                    _cache.Set(cacheKey, product, TimeSpan.FromHours(1));
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting product {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateProductAsync(ProductDto product)
        {
            try
            {
                var result = await _httpClient.PostAsync<ProductDto>("products", product);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(int id, ProductDto product)
        {
            try
            {
                var result = await _httpClient.PutAsync<ProductDto>($"products/{id}", product);
                if (result != null)
                {
                    _cache.Remove($"product_{id}");
                }
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"products/{id}");
                if (result)
                {
                    _cache.Remove($"product_{id}");
                }
                return result;
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
                // For bool returns, we make the request and return success status
                // PutAsync<T> is for deserializable responses, so we use a simple object
                var result = await _httpClient.PutAsync<dynamic>($"products/{id}/stock", new { quantity });
                if (result != null)
                {
                    _cache.Remove($"product_{id}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating stock for product {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ProductDto>?> GetByCategoryAsync(int categoryId)
        {
            try
            {
                var cacheKey = $"products_category_{categoryId}";
                var cached = _cache.Get<List<ProductDto>>(cacheKey);
                if (cached != null)
                    return cached;

                var products = await _httpClient.GetAsync<List<ProductDto>>($"products/category/{categoryId}");
                if (products != null)
                {
                    _cache.Set(cacheKey, products, TimeSpan.FromHours(2));
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting products by category {categoryId}: {ex.Message}");
                return null;
            }
        }
    }
}
