using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Features.Users.Commands.AddFavourite;
using mtkpm.Application.Features.Users.Commands.RemoveFavourite;
using mtkpm.Application.Features.Users.Commands.UpdateUser;
using mtkpm.Application.Features.Users.Queries.GetUserFavourites;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public UsersController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Lấy thông tin user hiện tại
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public IActionResult GetCurrentUser()
        {
            var userInfo = new
            {
                Id = _currentUserService.UserId,
                UserName = _currentUserService.UserName,
                Email = _currentUserService.Email,
                Roles = _currentUserService.Roles
            };

            return Ok(ApiResponse<object>.SuccessResponse(userInfo));
        }

        /// <summary>
        /// Cập nhật thông tin user
        /// </summary>
        [HttpPut("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new UpdateUserCommand
            {
                Id = userId,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<UserDto>.SuccessResponse(result, "Cập nhật thông tin thành công"));
        }

        /// <summary>
        /// Lấy danh sách sản phẩm yêu thích
        /// </summary>
        [HttpGet("favourites")]
        [ProducesResponseType(typeof(IEnumerable<FavouriteProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavourites()
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserFavouritesQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<FavouriteProductDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Thêm sản phẩm vào danh sách yêu thích
        /// </summary>
        [HttpPost("favourites")]
        [ProducesResponseType(typeof(FavouriteProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFavourite([FromBody] AddFavouriteProductDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new AddFavouriteCommand
            {
                UserId = userId,
                ProductId = dto.ProductId
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetFavourites), null, ApiResponse<FavouriteProductDto>.SuccessResponse(result, "Thêm vào yêu thích thành công"));
        }

        /// <summary>
        /// Xóa sản phẩm khỏi danh sách yêu thích
        /// </summary>
        [HttpDelete("favourites/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFavourite(int productId)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new RemoveFavouriteCommand(userId, productId);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã xóa khỏi yêu thích"));
        }
    }
}
