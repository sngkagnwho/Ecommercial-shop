using mtkpm.Domain.Events;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Observer Interface - L?ng nghe các domain events
    /// Observer Pattern - Observers implement interface này
    /// </summary>
    public interface INotificationObserver
    {
        /// <summary>
        /// X? lý event khi ??n hàng ???c t?o
        /// </summary>
        Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý event khi ??n hàng ???c xác nh?n
        /// </summary>
        Task OnOrderConfirmedAsync(OrderConfirmedEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý event khi ??n hàng ???c g?i ?i
        /// </summary>
        Task OnOrderShippedAsync(OrderShippedEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý event khi ??n hàng ???c giao
        /// </summary>
        Task OnOrderDeliveredAsync(OrderDeliveredEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý event khi ??n hàng b? h?y
        /// </summary>
        Task OnOrderCancelledAsync(OrderCancelledEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý event khi thanh toán thành công
        /// </summary>
        Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý event khi thanh toán th?t b?i
        /// </summary>
        Task OnPaymentFailedAsync(PaymentFailedEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý event khi hoàn ti?n
        /// </summary>
        Task OnPaymentRefundedAsync(PaymentRefundedEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tên observer ?? tracking
        /// </summary>
        string ObserverName { get; }
    }
}
