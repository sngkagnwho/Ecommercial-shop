using System.ComponentModel.DataAnnotations;

namespace mtkpm.Admin.Models.Notification
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string Type { get; set; } = "";
        public int? UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SendNotificationViewModel
    {
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string Type { get; set; } = "";
        public int? UserId { get; set; }
        public List<int>? UserIds { get; set; }
        public bool SendToAll { get; set; }
    }

    /// <summary>
    /// Notification method (Email, SMS, Push, Webhook)
    /// </summary>
    public class NotificationMethodViewModel
    {
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        [Range(0, int.MaxValue)]
        public int SubscriberCount { get; set; }
    }

    /// <summary>
    /// Notification method subscription request
    /// </summary>
    public class NotificationSubscriptionRequest
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        public string WebhookUrl { get; set; }

        [StringLength(500)]
        public string PushToken { get; set; }

        public bool SubscribeToAll { get; set; } = true;

        [StringLength(500)]
        public string EventFilter { get; set; }
    }

    /// <summary>
    /// Notification subscriber
    /// </summary>
    public class NotificationSubscriberViewModel
    {
        public int Id { get; set; }

        [Required]
        public string NotificationMethod { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        public string WebhookUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime SubscribedAt { get; set; }

        public int TotalNotificationsSent { get; set; }

        public int FailedNotifications { get; set; }

        public double SuccessRate => TotalNotificationsSent > 0 
            ? ((TotalNotificationsSent - FailedNotifications) * 100.0 / TotalNotificationsSent)
            : 100.0;
    }

    /// <summary>
    /// Test notification result
    /// </summary>
    public class NotificationTestResultViewModel
    {
        public string EventType { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(500)]
        public string Details { get; set; }

        public int NotificationsSent { get; set; }

        public int NotificationsFailed { get; set; }
    }

    /// <summary>
    /// Notification implementation guide
    /// </summary>
    public class NotificationGuideViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [StringLength(5000)]
        public string Implementation { get; set; }

        [StringLength(5000)]
        public string Examples { get; set; }

        public List<string> SupportedMethods { get; set; }

        public List<string> AvailableEvents { get; set; }

        [StringLength(1000)]
        public string ApiDocumentation { get; set; }
    }

    /// <summary>
    /// Notification console dashboard statistics
    /// </summary>
    public class NotificationStatisticsViewModel
    {
        public int TotalMethods { get; set; }

        public int ActiveMethods { get; set; }

        public int TotalSubscribers { get; set; }

        public int NotificationsSentToday { get; set; }

        public int NotificationsFailedToday { get; set; }

        public double TodaySuccessRate => NotificationsSentToday > 0
            ? ((NotificationsSentToday - NotificationsFailedToday) * 100.0 / NotificationsSentToday)
            : 100.0;

        public List<NotificationEventStatistic> EventStatistics { get; set; } = new();
    }

    /// <summary>
    /// Statistics for a specific notification event
    /// </summary>
    public class NotificationEventStatistic
    {
        public string EventName { get; set; }

        public int TotalSent { get; set; }

        public int Failed { get; set; }

        public double SuccessRate => TotalSent > 0
            ? ((TotalSent - Failed) * 100.0 / TotalSent)
            : 100.0;
    }

    /// <summary>
    /// Test event request
    /// </summary>
    public class TestEventRequest
    {
        public string EventType { get; set; }

        [StringLength(500)]
        public string TestData { get; set; }
    }

    /// <summary>
    /// Notification console - Event log entry
    /// </summary>
    public class NotificationEventLogViewModel
    {
        public int Id { get; set; }

        public string EventType { get; set; }

        public string NotificationMethod { get; set; }

        public string RecipientEmail { get; set; }

        public string RecipientPhone { get; set; }

        public bool Success { get; set; }

        [StringLength(500)]
        public string ErrorMessage { get; set; }

        public DateTime SentAt { get; set; }

        [StringLength(1000)]
        public string EventData { get; set; }
    }
}
