namespace mtkpm.Admin.Models.Order
{
    /// <summary>
    /// Status item for dropdown display
    /// </summary>
    public class StatusItem
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Badge { get; set; }
    }

    /// <summary>
    /// Order Status Enumeration
    /// </summary>
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Processing = 3,
        Shipping = 4,
        Delivered = 5,
        Completed = 6,
        Cancelled = 7,
        Returned = 8,
        Failed = 9
    }

    /// <summary>
    /// Payment Method Type Enumeration
    /// </summary>
    public enum PaymentMethodType
    {
        CreditCard = 1,
        DebitCard = 2,
        BankTransfer = 3,
        PayPal = 4,
        COD = 5,
        MobileWallet = 6
    }

    /// <summary>
    /// Main order view model for display list
    /// </summary>
    public class OrderListViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public string StatusDisplay => ((OrderStatus)Status).ToString();
        public bool IsPaid { get; set; }
        public string PaymentMethodDisplay { get; set; }

        public string StatusBadge => Status switch
        {
            1 => "badge-warning",
            2 => "badge-info",
            3 => "badge-primary",
            4 => "badge-secondary",
            5 => "badge-success",
            6 => "badge-success",
            7 => "badge-danger",
            8 => "badge-orange",
            9 => "badge-dark",
            _ => "badge-secondary"
        };

        public string PaymentBadge => IsPaid ? "badge-success" : "badge-warning";
    }

    /// <summary>
    /// Order detail view model (with items)
    /// </summary>
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }

        // Addresses
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }

        // Amounts
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        // Status
        public int Status { get; set; }
        public string StatusDisplay => ((OrderStatus)Status).ToString();
        public int PaymentMethod { get; set; }
        public string PaymentMethodDisplay => ((PaymentMethodType)PaymentMethod).ToString();
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }

        // Items
        public List<OrderItemDetailViewModel> OrderItems { get; set; } = new();

        // Notes
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // UI Helpers
        public string StatusBadge => Status switch
        {
            1 => "badge-warning",
            2 => "badge-info",
            3 => "badge-primary",
            4 => "badge-secondary",
            5 => "badge-success",
            6 => "badge-success",
            7 => "badge-danger",
            8 => "badge-orange",
            9 => "badge-dark",
            _ => "badge-secondary"
        };

        public string PaymentBadge => IsPaid ? "badge-success" : "badge-warning";

        public bool CanCancel => Status != (int)OrderStatus.Shipping && 
                                 Status != (int)OrderStatus.Delivered && 
                                 Status != (int)OrderStatus.Completed;
    }

    /// <summary>
    /// Order item detail view model
    /// </summary>
    public class OrderItemDetailViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// Order statistics for dashboard
    /// </summary>
    public class OrderStatisticsViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int UnpaidOrders { get; set; }
    }

    /// <summary>
    /// Create order view model
    /// </summary>
    public class CreateOrderViewModel
    {
        public int UserId { get; set; }
        public int SavedAddressId { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public int PaymentMethod { get; set; }
        public string Note { get; set; }
        public List<CreateOrderItemViewModel> OrderItems { get; set; } = new();
    }

    /// <summary>
    /// Create order item view model (for order creation)
    /// </summary>
    public class CreateOrderItemViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Update order status request
    /// </summary>
    public class UpdateOrderStatusViewModel
    {
        public int Status { get; set; }
    }

    /// <summary>
    /// Search/filter orders view model
    /// </summary>
    public class SearchOrdersViewModel
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public int? Status { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
