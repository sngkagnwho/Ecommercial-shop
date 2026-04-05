using FluentValidation;

namespace mtkpm.Application.Features.Users.Commands.CreateUserAddress
{
    public class CreateUserAddressCommandValidator : AbstractValidator<CreateUserAddressCommand>
    {
        public CreateUserAddressCommandValidator()
        {
            RuleFor(x => x.ReceiverName)
                .NotEmpty().WithMessage("Tên ng??i nh?n là b?t bu?c")
                .MaximumLength(100).WithMessage("Tên ng??i nh?n không ???c v??t quá 100 ký t?");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("S? ?i?n tho?i là b?t bu?c")
                .Matches(@"^(\+84|0)[0-9]{9,10}$").WithMessage("S? ?i?n tho?i không h?p l?");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("???ng/ph? là b?t bu?c")
                .MaximumLength(200).WithMessage("???ng/ph? không ???c v??t quá 200 ký t?");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("Qu?n/huy?n là b?t bu?c")
                .MaximumLength(100).WithMessage("Qu?n/huy?n không ???c v??t quá 100 ký t?");

            RuleFor(x => x.Ward)
                .NotEmpty().WithMessage("Ph??ng/xã là b?t bu?c")
                .MaximumLength(100).WithMessage("Ph??ng/xã không ???c v??t quá 100 ký t?");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Thành ph? là b?t bu?c")
                .MaximumLength(100).WithMessage("Thành ph? không ???c v??t quá 100 ký t?");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Mã b?u ?i?n là b?t bu?c")
                .MaximumLength(20).WithMessage("Mã b?u ?i?n không ???c v??t quá 20 ký t?");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Qu?c gia là b?t bu?c")
                .MaximumLength(100).WithMessage("Qu?c gia không ???c v??t quá 100 ký t?");

            RuleFor(x => x.Label)
                .NotEmpty().WithMessage("Nhãn ??a ch? là b?t bu?c")
                .MaximumLength(50).WithMessage("Nhãn không ???c v??t quá 50 ký t?");
        }
    }
}
