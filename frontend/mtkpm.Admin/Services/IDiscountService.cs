using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.Discount;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for discount management service
    /// </summary>
    public interface IDiscountService
    {
        Task<List<DiscountViewModel>?> GetAllDiscountsAsync();
        Task<DiscountViewModel?> GetDiscountByIdAsync(int id);
        Task<DiscountViewModel?> CreateDiscountAsync(CreateDiscountViewModel request);
        Task<DiscountViewModel?> UpdateDiscountAsync(int id, UpdateDiscountViewModel request);
        Task<bool> DeleteDiscountAsync(int id);
        Task<DiscountViewModel?> GetDiscountByCodeAsync(string code);
    }

    /// <summary>
    /// Implementation of discount service
    /// </summary>
    public class DiscountService : IDiscountService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IApiService apiService, ILogger<DiscountService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<DiscountViewModel>?> GetAllDiscountsAsync()
        {
            try
            {
                return await _apiService.GetAsync<List<DiscountViewModel>>(ApiEndpoints.Discounts.GetAll);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting discounts: {ex.Message}");
                return null;
            }
        }

        public async Task<DiscountViewModel?> GetDiscountByIdAsync(int id)
        {
            try
            {
                return await _apiService.GetAsync<DiscountViewModel>($"/discounts/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting discount {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<DiscountViewModel?> CreateDiscountAsync(CreateDiscountViewModel request)
        {
            try
            {
                return await _apiService.PostAsync<DiscountViewModel>(ApiEndpoints.Discounts.Base, request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating discount: {ex.Message}");
                return null;
            }
        }

        public async Task<DiscountViewModel?> UpdateDiscountAsync(int id, UpdateDiscountViewModel request)
        {
            try
            {
                return await _apiService.PutAsync<DiscountViewModel>($"/discounts/{id}", request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating discount {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteDiscountAsync(int id)
        {
            try
            {
                return await _apiService.DeleteAsync($"/discounts/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting discount {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<DiscountViewModel?> GetDiscountByCodeAsync(string code)
        {
            try
            {
                return await _apiService.GetAsync<DiscountViewModel>($"/discounts/code/{code}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting discount by code {code}: {ex.Message}");
                return null;
            }
        }
    }
}
