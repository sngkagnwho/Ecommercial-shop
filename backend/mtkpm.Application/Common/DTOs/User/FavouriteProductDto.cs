using System;

namespace mtkpm.Application.Common.DTOs.User
{
    public class FavouriteProductDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }
        public int ProductStockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
