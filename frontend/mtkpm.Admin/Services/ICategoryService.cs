using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.Category;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for category management service
    /// </summary>
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>?> GetAllCategoriesAsync();
        Task<CategoryViewModel?> GetCategoryByIdAsync(int id);
        Task<CategoryViewModel?> CreateCategoryAsync(CreateCategoryViewModel request);
        Task<CategoryViewModel?> UpdateCategoryAsync(int id, UpdateCategoryViewModel request);
        Task<bool> DeleteCategoryAsync(int id);
    }

    /// <summary>
    /// Implementation of category service
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IApiService apiService, ILogger<CategoryService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<CategoryViewModel>?> GetAllCategoriesAsync()
        {
            try
            {
                return await _apiService.GetAsync<List<CategoryViewModel>>(ApiEndpoints.Categories.GetAll);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting categories: {ex.Message}");
                return null;
            }
        }

        public async Task<CategoryViewModel?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _apiService.GetAsync<CategoryViewModel>($"/categories/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting category {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<CategoryViewModel?> CreateCategoryAsync(CreateCategoryViewModel request)
        {
            try
            {
                return await _apiService.PostAsync<CategoryViewModel>(ApiEndpoints.Categories.Base, request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                return null;
            }
        }

        public async Task<CategoryViewModel?> UpdateCategoryAsync(int id, UpdateCategoryViewModel request)
        {
            try
            {
                return await _apiService.PutAsync<CategoryViewModel>($"/categories/{id}", request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                return await _apiService.DeleteAsync($"/categories/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category {id}: {ex.Message}");
                return false;
            }
        }
    }
}
