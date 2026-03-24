using System;
using System.Collections.Generic;

namespace mtkpm.Application.Common.DTOs.Cart
{
    public class CartDto
    {
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
