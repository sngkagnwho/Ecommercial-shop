using mtkpm.Domain.Enums.Business;

namespace mtkpm.Domain.Events
{
    /// <summary>
    /// Event khi ??n hàng ???c t?o
    /// </summary>
    public class OrderCreatedEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }

        public OrderCreatedEvent(int orderId, int userId, string orderNumber, decimal totalAmount, string shippingAddress)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            OrderNumber = orderNumber;
            TotalAmount = totalAmount;
            ShippingAddress = shippingAddress;
        }
    }

    /// <summary>
    /// Event khi ??n hàng ???c xác nh?n và s?p g?i
    /// </summary>
    public class OrderConfirmedEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }

        public OrderConfirmedEvent(int orderId, int userId, string orderNumber)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            OrderNumber = orderNumber;
        }
    }

    /// <summary>
    /// Event khi ??n hàng ???c g?i ?i
    /// </summary>
    public class OrderShippedEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public string TrackingNumber { get; set; }

        public OrderShippedEvent(int orderId, int userId, string orderNumber, string trackingNumber)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            OrderNumber = orderNumber;
            TrackingNumber = trackingNumber;
        }
    }

    /// <summary>
    /// Event khi ??n hàng ???c giao thành công
    /// </summary>
    public class OrderDeliveredEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DeliveryDate { get; set; }

        public OrderDeliveredEvent(int orderId, int userId, string orderNumber, DateTime deliveryDate)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            OrderNumber = orderNumber;
            DeliveryDate = deliveryDate;
        }
    }

    /// <summary>
    /// Event khi ??n hàng b? h?y
    /// </summary>
    public class OrderCancelledEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public string Reason { get; set; }

        public OrderCancelledEvent(int orderId, int userId, string orderNumber, string reason)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            OrderNumber = orderNumber;
            Reason = reason;
        }
    }
}
