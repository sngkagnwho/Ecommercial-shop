using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Users.Queries.GetUserAddresses
{
    public class GetUserAddressesQueryHandler : IRequestHandler<GetUserAddressesQuery, IEnumerable<UserAddressDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserAddressesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserAddressDto>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _unitOfWork.UserAddresses.GetByUserIdAsync(request.UserId, cancellationToken);
            return _mapper.Map<IEnumerable<UserAddressDto>>(addresses);
        }
    }
}
