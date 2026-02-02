using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.DTOs.Order
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "??a ch? giao hàng là b?t bu?c")]
        [StringLength(500, ErrorMessage = "??a ch? giao hàng không ???c v??t quá 500 ký t?")]
        public string ShippingAddress { get; set; }

        [StringLength(500, ErrorMessage = "??a ch? thanh toán không ???c v??t quá 500 ký t?")]
        public string? BillingAddress { get; set; }

        [Required(ErrorMessage = "Ph??ng th?c thanh toán là b?t bu?c")]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú không ???c v??t quá 500 ký t?")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Danh sách s?n ph?m là b?t bu?c")]
        [MinLength(1, ErrorMessage = "??n hàng ph?i có ít nh?t 1 s?n ph?m")]
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "ID s?n ph?m là b?t bu?c")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "S? l??ng là b?t bu?c")]
        [Range(1, int.MaxValue, ErrorMessage = "S? l??ng ph?i l?n h?n 0")]
        public int Quantity { get; set; }
    }
}
