using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} not found");
            }

            if (!string.IsNullOrEmpty(request.UserName) && request.UserName != user.UserName)
            {
                var existingUser = await _userManager.FindByNameAsync(request.UserName);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    throw new InvalidOperationException($"Username '{request.UserName}' is already taken");
                }
                user.UserName = request.UserName;
            }

            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    throw new InvalidOperationException($"Email '{request.Email}' is already registered");
                }
                user.Email = request.Email;
                user.EmailConfirmed = false;
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber;
            }

            user.SetUpdated(null);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update user: {errors}");
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
