namespace mtkpm.Admin.Core.Enums
{
    /// <summary>
    /// User roles in admin system
    /// </summary>
    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        Staff = 3,
        Supervisor = 4
    }

    /// <summary>
    /// Order status enum
    /// </summary>
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Refunded = 6
    }

    /// <summary>
    /// Payment status enum
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4,
        Refunded = 5
    }

    /// <summary>
    /// Notification type enum
    /// </summary>
    public enum NotificationType
    {
        Order = 1,
        Product = 2,
        Payment = 3,
        System = 4,
        User = 5
    }
}
