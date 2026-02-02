using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Product;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<PaginatedListDto<ProductDto>> GetPaginatedAsync(int pageIndex, int pageSize);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> SearchAsync(string searchTerm);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStockAsync(int id, int quantity);
    }
}
