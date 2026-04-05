using mtkpm.Admin.Features.Products.Models;

namespace mtkpm.Admin.Features.Products.Services
{
    /// <summary>
    /// Interface for product management service
    /// </summary>
    public interface IProductFeatureService
    {
        /// <summary>
        /// Get paginated products list
        /// </summary>
        Task<(List<ProductDto> Items, int TotalCount)?> GetProductsAsync(int pageIndex, int pageSize, string? searchTerm = null, int? categoryId = null);

        /// <summary>
        /// Get product by ID
        /// </summary>
        Task<ProductDetailDto?> GetProductByIdAsync(int id);

        /// <summary>
        /// Create new product
        /// </summary>
        Task<bool> CreateProductAsync(ProductDto product);

        /// <summary>
        /// Update product
        /// </summary>
        Task<bool> UpdateProductAsync(int id, ProductDto product);

        /// <summary>
        /// Delete product
        /// </summary>
        Task<bool> DeleteProductAsync(int id);

        /// <summary>
        /// Update product stock
        /// </summary>
        Task<bool> UpdateStockAsync(int id, int quantity);

        /// <summary>
        /// Get products by category
        /// </summary>
        Task<List<ProductDto>?> GetByCategoryAsync(int categoryId);
    }
}
