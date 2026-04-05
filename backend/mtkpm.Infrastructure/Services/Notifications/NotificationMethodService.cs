using mtkpm.Application.Common.DTOs.Notification;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Infrastructure.Services.Notifications
{
    public class NotificationMethodService : INotificationMethodService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly EmailNotificationService _emailNotificationService;
        private readonly SMSNotificationService _smsNotificationService;
        private readonly PushNotificationService _pushNotificationService;

        public NotificationMethodService(
            IEventPublisher eventPublisher,
            EmailNotificationService emailNotificationService,
            SMSNotificationService smsNotificationService,
            PushNotificationService pushNotificationService)
        {
            _eventPublisher = eventPublisher;
            _emailNotificationService = emailNotificationService;
            _smsNotificationService = smsNotificationService;
            _pushNotificationService = pushNotificationService;
        }

        public List<NotificationMethodDto> GetMethods()
        {
            var subscriberNames = _eventPublisher.GetSubscriberNames();
            return new List<NotificationMethodDto>
            {
                BuildMethodInfo(_emailNotificationService.ObserverName, "Email", "G?i thông báo qua email", subscriberNames),
                BuildMethodInfo(_smsNotificationService.ObserverName, "SMS", "G?i thông báo qua SMS", subscriberNames),
                BuildMethodInfo(_pushNotificationService.ObserverName, "Push", "G?i push notification", subscriberNames)
            };
        }

        public bool Subscribe(string methodName)
        {
            var observer = ResolveObserver(methodName);
            if (observer == null)
            {
                return false;
            }

            _eventPublisher.Subscribe(observer);
            return true;
        }

        public bool Unsubscribe(string methodName)
        {
            var observer = ResolveObserver(methodName);
            if (observer == null)
            {
                return false;
            }

            _eventPublisher.Unsubscribe(observer);
            return true;
        }

        private NotificationMethodDto BuildMethodInfo(
            string observerName,
            string displayName,
            string description,
            List<string> subscriberNames)
        {
            return new NotificationMethodDto
            {
                MethodKey = observerName,
                Name = displayName,
                Description = description,
                IsActive = subscriberNames.Contains(observerName)
            };
        }

        private INotificationObserver? ResolveObserver(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                return null;
            }

            return methodName.Trim().ToLowerInvariant() switch
            {
                "email" or "emailnotification" => _emailNotificationService,
                "sms" or "smsnotification" => _smsNotificationService,
                "push" or "pushnotification" => _pushNotificationService,
                _ => null
            };
        }
    }
}
