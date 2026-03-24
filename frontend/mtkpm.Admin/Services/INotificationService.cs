using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.Notification;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for notification service
    /// </summary>
    public interface INotificationService
    {
        Task<List<NotificationViewModel>?> GetNotificationsAsync();
        Task<NotificationViewModel?> GetNotificationByIdAsync(int id);
        Task<bool> SendNotificationAsync(SendNotificationViewModel request);
        Task<bool> DeleteNotificationAsync(int id);
        Task<bool> MarkAsReadAsync(int notificationId);
    }

    /// <summary>
    /// Implementation of notification service
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IApiService apiService, ILogger<NotificationService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<NotificationViewModel>?> GetNotificationsAsync()
        {
            try
            {
                return await _apiService.GetAsync<List<NotificationViewModel>>(ApiEndpoints.Notifications.GetAll);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting notifications: {ex.Message}");
                return null;
            }
        }

        public async Task<NotificationViewModel?> GetNotificationByIdAsync(int id)
        {
            try
            {
                return await _apiService.GetAsync<NotificationViewModel>($"/notifications/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting notification {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> SendNotificationAsync(SendNotificationViewModel request)
        {
            try
            {
                var result = await _apiService.PostAsync<object>(ApiEndpoints.Notifications.Send, request);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending notification: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            try
            {
                return await _apiService.DeleteAsync($"/notifications/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting notification {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var result = await _apiService.PutAsync<object>($"/notifications/{notificationId}/mark-as-read", null);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking notification {notificationId} as read: {ex.Message}");
                return false;
            }
        }
    }
}
