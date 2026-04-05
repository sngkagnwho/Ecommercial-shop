namespace mtkpm.Admin.Features.Orders.Models
{
    /// <summary>
    /// Order list item DTO
    /// </summary>
    public class OrderDto
    {
        public int Id { get; set; }
        public string? OrderCode { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int ItemCount { get; set; }
    }

    /// <summary>
    /// Order detail DTO
    /// </summary>
    public class OrderDetailDto : OrderDto
    {
        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Order item DTO
    /// </summary>
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
