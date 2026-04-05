using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Users.Queries.GetUserAddressById
{
    public class GetUserAddressByIdQueryHandler : IRequestHandler<GetUserAddressByIdQuery, UserAddressDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserAddressByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserAddressDto?> Handle(GetUserAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.UserAddresses
                .GetByIdAndUserIdAsync(request.AddressId, request.UserId, cancellationToken);

            return address == null ? null : _mapper.Map<UserAddressDto>(address);
        }
    }
}
