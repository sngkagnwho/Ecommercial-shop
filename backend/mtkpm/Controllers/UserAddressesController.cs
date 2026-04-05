using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Features.Users.Commands.CreateUserAddress;
using mtkpm.Application.Features.Users.Commands.DeleteUserAddress;
using mtkpm.Application.Features.Users.Commands.UpdateUserAddress;
using mtkpm.Application.Features.Users.Queries.GetUserAddressById;
using mtkpm.Application.Features.Users.Queries.GetUserAddresses;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class UserAddressesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public UserAddressesController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Lấy danh sách tất cả địa chỉ của người dùng hiện tại
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserAddressesQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<UserAddressDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy chi tiết một địa chỉ của người dùng
        /// </summary>
        [HttpGet("{addressId:int}")]
        [ProducesResponseType(typeof(UserAddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyAddress(int addressId)
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserAddressByIdQuery(addressId, userId);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(ApiResponse<UserAddressDto>.FailureResponse("Địa chỉ không tìm thấy"));
            }

            return Ok(ApiResponse<UserAddressDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Thêm địa chỉ mới cho người dùng
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserAddressDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAddress([FromBody] CreateUserAddressDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new CreateUserAddressCommand
            {
                UserId = userId,
                ReceiverName = dto.ReceiverName,
                PhoneNumber = dto.PhoneNumber,
                Street = dto.Street,
                District = dto.District,
                Ward = dto.Ward,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Label = dto.Label,
                IsDefault = dto.IsDefault
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMyAddress), new { addressId = result.Id },
                ApiResponse<UserAddressDto>.SuccessResponse(result, "Thêm địa chỉ thành công"));
        }

        /// <summary>
        /// Cập nhật địa chỉ của người dùng
        /// </summary>
        [HttpPut("{addressId:int}")]
        [ProducesResponseType(typeof(UserAddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] UpdateUserAddressDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new UpdateUserAddressCommand
            {
                AddressId = addressId,
                UserId = userId,
                ReceiverName = dto.ReceiverName,
                PhoneNumber = dto.PhoneNumber,
                Street = dto.Street,
                District = dto.District,
                Ward = dto.Ward,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Label = dto.Label,
                IsDefault = dto.IsDefault
            };

            try
            {
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<UserAddressDto>.SuccessResponse(result, "Cập nhật địa chỉ thành công"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ApiResponse<UserAddressDto>.FailureResponse(ex.Message));
            }
        }

        /// <summary>
        /// Xóa địa chỉ của người dùng
        /// </summary>
        [HttpDelete("{addressId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new DeleteUserAddressCommand
            {
                AddressId = addressId,
                UserId = userId
            };

            try
            {
                await _mediator.Send(command);
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Xóa địa chỉ thành công"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ApiResponse<bool>.FailureResponse(ex.Message));
            }
        }
    }
}
