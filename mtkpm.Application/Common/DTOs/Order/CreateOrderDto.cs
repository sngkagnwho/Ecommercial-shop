using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.DTOs.Order
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc")]
        [StringLength(500, ErrorMessage = "Địa chỉ giao hàng không được vượt quá 500 ký tự")]
        public string ShippingAddress { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ thanh toán không được vượt quá 500 ký tự")]
        public string? BillingAddress { get; set; }

        [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc")]
        public PaymentMethodType PaymentMethod { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Danh sách sản phẩm là bắt buộc")]
        [MinLength(1, ErrorMessage = "Đơn hàng phải có ít nhất 1 sản phẩm")]
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "ID sản phẩm là bắt buộc")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
}
