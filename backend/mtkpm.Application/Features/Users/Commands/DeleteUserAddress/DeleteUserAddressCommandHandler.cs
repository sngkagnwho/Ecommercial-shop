using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Users.Commands.DeleteUserAddress
{
    public class DeleteUserAddressCommandHandler : IRequestHandler<DeleteUserAddressCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserAddressCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserAddressCommand request, CancellationToken cancellationToken)
        {
            var userAddress = await _unitOfWork.UserAddresses
                .GetByIdAndUserIdAsync(request.AddressId, request.UserId, cancellationToken);

            if (userAddress == null)
            {
                throw new InvalidOperationException("??a ch? không tìm th?y ho?c không thu?c v? ng??i dùng này");
            }

            _unitOfWork.UserAddresses.Remove(userAddress);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
