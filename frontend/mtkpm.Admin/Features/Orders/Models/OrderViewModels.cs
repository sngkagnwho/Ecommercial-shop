namespace mtkpm.Admin.Features.Orders.Models
{
    /// <summary>
    /// Order view model for displaying order information in the admin interface
    /// Maps to backend OrderDto
    /// </summary>
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string OrderNumber { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; } = "";
        public string? BillingAddress { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Order status as integer (matches backend OrderStatus enum:
        /// Pending=1, Confirmed=2, Processing=3, Shipping=4, Delivered=5, Completed=6, Cancelled=7, Returned=8, Failed=9)
        /// </summary>
        public int Status { get; set; }
        public string StatusDisplay { get; set; } = "";
        /// <summary>
        /// Payment method as integer value
        /// </summary>
        public int PaymentMethod { get; set; }
        public string PaymentMethodDisplay { get; set; } = "";
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? Note { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Order item view model for displaying individual line items
    /// Maps to backend OrderItemDto
    /// </summary>
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
