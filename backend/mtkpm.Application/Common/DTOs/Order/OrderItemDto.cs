using System;

namespace mtkpm.Application.Common.DTOs.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
