namespace mtkpm.Admin.Models.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = "";
        public int UserId { get; set; }
        public string UserEmail { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = "";
        public string? BillingAddress { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string? Note { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class UpdateOrderStatusViewModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = "";
        public string? Note { get; set; }
    }

    public class OrderFilterViewModel
    {
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
