using mtkpm.Admin.Features.Users.Models;

namespace mtkpm.Admin.Features.Users.Services
{
    /// <summary>
    /// Interface for user management service
    /// </summary>
    public interface IUserFeatureService
    {
        /// <summary>
        /// Get paginated users list
        /// </summary>
        Task<(List<UserDto> Items, int TotalCount)?> GetUsersAsync(int pageIndex, int pageSize, string? searchTerm = null, bool? isActive = null);

        /// <summary>
        /// Get user detail by ID
        /// </summary>
        Task<UserDetailDto?> GetUserByIdAsync(int id);

        /// <summary>
        /// Create new user
        /// </summary>
        Task<bool> CreateUserAsync(UserDto user);

        /// <summary>
        /// Update user
        /// </summary>
        Task<bool> UpdateUserAsync(int id, UserDto user);

        /// <summary>
        /// Delete user
        /// </summary>
        Task<bool> DeleteUserAsync(int id);

        /// <summary>
        /// Toggle user active status
        /// </summary>
        Task<bool> ToggleUserStatusAsync(int id);

        /// <summary>
        /// Get total users count
        /// </summary>
        Task<int> GetTotalUsersCountAsync();

        /// <summary>
        /// Search users by criteria
        /// </summary>
        Task<List<UserDto>?> SearchUsersAsync(string searchTerm);
    }
}
