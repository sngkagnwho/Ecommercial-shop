using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace mtkpm.Infrastructure.Services.Notifications
{
    /// <summary>
    /// Notification Subscriber - Tự động đăng ký observers khi ứng dụng khởi động
    /// </summary>
    public class NotificationSubscriber : IHostedService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerService _logger;

        public NotificationSubscriber(
            IEventPublisher eventPublisher,
            IServiceProvider serviceProvider,
            ILoggerService logger)
        {
            _eventPublisher = eventPublisher;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInfo("✔ Starting Notification Subscriber", "NotificationSubscriber");

            // Tự động đăng ký tất cả observers
            using (var scope = _serviceProvider.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<EmailNotificationService>();
                var smsService = scope.ServiceProvider.GetRequiredService<SMSNotificationService>();
                var pushService = scope.ServiceProvider.GetRequiredService<PushNotificationService>();

                _eventPublisher.Subscribe(emailService);
                _eventPublisher.Subscribe(smsService);
                _eventPublisher.Subscribe(pushService);
            }

            _logger.LogInfo($"✔ Registered observers: {string.Join(", ", _eventPublisher.GetSubscriberNames())}", "NotificationSubscriber");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInfo("✔ Stopping Notification Subscriber", "NotificationSubscriber");
            return Task.CompletedTask;
        }
    }
}

