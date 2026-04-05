using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Users.Commands.UpdateUserAddress
{
    public class UpdateUserAddressCommandHandler : IRequestHandler<UpdateUserAddressCommand, UserAddressDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserAddressDto> Handle(UpdateUserAddressCommand request, CancellationToken cancellationToken)
        {
            var userAddress = await _unitOfWork.UserAddresses
                .GetByIdAndUserIdAsync(request.AddressId, request.UserId, cancellationToken);

            if (userAddress == null)
            {
                throw new InvalidOperationException("??a ch? không tìm th?y ho?c không thu?c v? ng??i dùng này");
            }

            // N?u là ??a ch? m?c ??nh, unset ??a ch? m?c ??nh hi?n t?i
            if (request.IsDefault && !userAddress.IsDefault)
            {
                var currentDefault = await _unitOfWork.UserAddresses
                    .GetDefaultAddressByUserIdAsync(request.UserId, cancellationToken);

                if (currentDefault != null)
                {
                    currentDefault.UnsetDefault();
                    _unitOfWork.UserAddresses.Update(currentDefault);
                }
            }

            // C?p nh?t ??a ch?
            userAddress.Update(
                request.ReceiverName,
                request.PhoneNumber,
                request.Street,
                request.District,
                request.Ward,
                request.City,
                request.PostalCode,
                request.Country,
                request.Label);

            if (request.IsDefault)
            {
                userAddress.SetAsDefault();
            }
            else
            {
                userAddress.UnsetDefault();
            }

            _unitOfWork.UserAddresses.Update(userAddress);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserAddressDto>(userAddress);
        }
    }
}
