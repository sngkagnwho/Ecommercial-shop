using mtkpm.Domain.Events;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Event Publisher (Subject) - Qu?n lý observers và phát events
    /// Observer Pattern - Subject ??ng ký/h?y observers và notify
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// ??ng ký observer ?? l?ng nghe events
        /// </summary>
        void Subscribe(INotificationObserver observer);

        /// <summary>
        /// H?y ??ng ký observer
        /// </summary>
        void Unsubscribe(INotificationObserver observer);

        /// <summary>
        /// Phát event t?i t?t c? observers
        /// </summary>
        Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// L?y s? l??ng observers ?ang l?ng nghe
        /// </summary>
        int GetSubscriberCount();

        /// <summary>
        /// L?y danh sách tên observers
        /// </summary>
        List<string> GetSubscriberNames();
    }
}
