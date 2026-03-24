using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.User;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for user management service
    /// </summary>
    public interface IUserService
    {
        Task<Models.PaginatedResponse<UserViewModel>?> GetUsersAsync(int pageIndex, int pageSize);
        Task<UserDetailViewModel?> GetUserByIdAsync(int id);
        Task<UserViewModel?> CreateUserAsync(CreateUserViewModel request);
        Task<UserViewModel?> UpdateUserAsync(int id, UpdateUserViewModel request);
        Task<bool> DeleteUserAsync(int id);
        Task<List<UserViewModel>?> SearchUsersAsync(string searchTerm);
    }

    /// <summary>
    /// Implementation of user service
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<UserService> _logger;

        public UserService(IApiService apiService, ILogger<UserService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<Models.PaginatedResponse<UserViewModel>?> GetUsersAsync(int pageIndex, int pageSize)
        {
            try
            {
                var endpoint = $"/users?pageIndex={pageIndex}&pageSize={pageSize}";
                return await _apiService.GetAsync<Models.PaginatedResponse<UserViewModel>>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting users: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDetailViewModel?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _apiService.GetAsync<UserDetailViewModel>($"/users/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<UserViewModel?> CreateUserAsync(CreateUserViewModel request)
        {
            try
            {
                return await _apiService.PostAsync<UserViewModel>(ApiEndpoints.Users.Base, request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return null;
            }
        }

        public async Task<UserViewModel?> UpdateUserAsync(int id, UpdateUserViewModel request)
        {
            try
            {
                return await _apiService.PutAsync<UserViewModel>($"/users/{id}", request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                return await _apiService.DeleteAsync($"/users/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserViewModel>?> SearchUsersAsync(string searchTerm)
        {
            try
            {
                return await _apiService.GetAsync<List<UserViewModel>>($"/users/search?term={searchTerm}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching users: {ex.Message}");
                return null;
            }
        }
    }
}
