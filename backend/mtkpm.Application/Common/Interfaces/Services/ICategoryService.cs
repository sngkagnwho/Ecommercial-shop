using mtkpm.Application.Common.DTOs.Category;
using mtkpm.Application.Common.DTOs.Common;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<PaginatedListDto<CategoryDto>> GetPaginatedAsync(int pageIndex, int pageSize);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
