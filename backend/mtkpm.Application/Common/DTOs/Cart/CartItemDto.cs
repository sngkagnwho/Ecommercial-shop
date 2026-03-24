using System;

namespace mtkpm.Application.Common.DTOs.Cart
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int AvailableStock { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
