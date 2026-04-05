namespace mtkpm.Application.Common.DTOs.Notification
{
    public class SubscribersResponseDto
    {
        public int TotalSubscribers { get; set; }
        public List<string> Subscribers { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
