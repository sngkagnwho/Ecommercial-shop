using mtkpm.Application.Common.DTOs.Notification;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface INotificationMethodService
    {
        List<NotificationMethodDto> GetMethods();
        bool Subscribe(string methodName);
        bool Unsubscribe(string methodName);
    }
}
