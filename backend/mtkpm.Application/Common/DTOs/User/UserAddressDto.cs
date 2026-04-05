using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.User
{
    public class CreateUserAddressDto
    {
        [Required(ErrorMessage = "Tên ng??i nh?n là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Tên ng??i nh?n không ???c v??t quá 100 ký t?")]
        public string ReceiverName { get; set; } = string.Empty;

        [Required(ErrorMessage = "S? ?i?n tho?i là b?t bu?c")]
        [RegularExpression(@"^(\+84|0)[0-9]{9,10}$", ErrorMessage = "S? ?i?n tho?i không h?p l?")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "???ng/ph? là b?t bu?c")]
        [StringLength(200, ErrorMessage = "???ng/ph? không ???c v??t quá 200 ký t?")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Qu?n/huy?n là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Qu?n/huy?n không ???c v??t quá 100 ký t?")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ph??ng/xã là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Ph??ng/xã không ???c v??t quá 100 ký t?")]
        public string Ward { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thành ph? là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Thành ph? không ???c v??t quá 100 ký t?")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã b?u ?i?n là b?t bu?c")]
        [StringLength(20, ErrorMessage = "Mã b?u ?i?n không ???c v??t quá 20 ký t?")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Qu?c gia là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Qu?c gia không ???c v??t quá 100 ký t?")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nhãn ??a ch? là b?t bu?c")]
        [StringLength(50, ErrorMessage = "Nhãn không ???c v??t quá 50 ký t?")]
        public string Label { get; set; } = "Khác";

        public bool IsDefault { get; set; } = false;
    }

    public class UpdateUserAddressDto
    {
        [Required(ErrorMessage = "ID ??a ch? là b?t bu?c")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên ng??i nh?n là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Tên ng??i nh?n không ???c v??t quá 100 ký t?")]
        public string ReceiverName { get; set; } = string.Empty;

        [Required(ErrorMessage = "S? ?i?n tho?i là b?t bu?c")]
        [RegularExpression(@"^(\+84|0)[0-9]{9,10}$", ErrorMessage = "S? ?i?n tho?i không h?p l?")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "???ng/ph? là b?t bu?c")]
        [StringLength(200, ErrorMessage = "???ng/ph? không ???c v??t quá 200 ký t?")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Qu?n/huy?n là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Qu?n/huy?n không ???c v??t quá 100 ký t?")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ph??ng/xã là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Ph??ng/xã không ???c v??t quá 100 ký t?")]
        public string Ward { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thành ph? là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Thành ph? không ???c v??t quá 100 ký t?")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã b?u ?i?n là b?t bu?c")]
        [StringLength(20, ErrorMessage = "Mã b?u ?i?n không ???c v??t quá 20 ký t?")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Qu?c gia là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Qu?c gia không ???c v??t quá 100 ký t?")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nhãn ??a ch? là b?t bu?c")]
        [StringLength(50, ErrorMessage = "Nhãn không ???c v??t quá 50 ký t?")]
        public string Label { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
    }

    public class UserAddressDto
    {
        public int Id { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
