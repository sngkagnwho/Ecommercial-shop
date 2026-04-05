using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.Users.Commands.CreateUserAddress
{
    public class CreateUserAddressCommandHandler : IRequestHandler<CreateUserAddressCommand, UserAddressDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserAddressDto> Handle(CreateUserAddressCommand request, CancellationToken cancellationToken)
        {
            // N?u là ??a ch? m?c ??nh, unset ??a ch? m?c ??nh hi?n t?i
            if (request.IsDefault)
            {
                var currentDefault = await _unitOfWork.UserAddresses
                    .GetDefaultAddressByUserIdAsync(request.UserId, cancellationToken);

                if (currentDefault != null)
                {
                    currentDefault.UnsetDefault();
                    _unitOfWork.UserAddresses.Update(currentDefault);
                }
            }

            // T?o ??a ch? m?i
            var userAddress = new UserAddress(
                request.UserId,
                request.ReceiverName,
                request.PhoneNumber,
                request.Street,
                request.District,
                request.Ward,
                request.City,
                request.PostalCode,
                request.Country,
                request.Label,
                request.IsDefault);

            await _unitOfWork.UserAddresses.AddAsync(userAddress, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserAddressDto>(userAddress);
        }
    }
}
