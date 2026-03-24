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
}
