using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Features.Users.Commands.AddFavourite;
using mtkpm.Application.Features.Users.Commands.RemoveFavourite;
using mtkpm.Application.Features.Users.Queries.GetUserFavourites;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class FavouriteProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public FavouriteProductsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Lấy danh sách tất cả sản phẩm yêu thích
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FavouriteProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyFavourites()
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserFavouritesQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<FavouriteProductDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Kiểm tra sản phẩm có trong danh sách yêu thích không
        /// </summary>
        [HttpGet("{productId}/check")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsProductFavourite(int productId)
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserFavouritesQuery(userId);
            var result = await _mediator.Send(query);
            var isFavourite = result.Any(f => f.ProductId == productId);
            return Ok(ApiResponse<bool>.SuccessResponse(isFavourite));
        }

        /// <summary>
        /// Thêm sản phẩm vào danh sách yêu thích
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FavouriteProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToFavourites([FromBody] AddFavouriteProductDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new AddFavouriteCommand
            {
                UserId = userId,
                ProductId = dto.ProductId
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMyFavourites), null, 
                ApiResponse<FavouriteProductDto>.SuccessResponse(result, "Thêm vào yêu thích thành công"));
        }

        /// <summary>
        /// Xóa sản phẩm khỏi danh sách yêu thích
        /// </summary>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromFavourites(int productId)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new RemoveFavouriteCommand(userId, productId);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã xóa khỏi yêu thích"));
        }

        /// <summary>
        /// Xóa tất cả sản phẩm yêu thích
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearAllFavourites()
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserFavouritesQuery(userId);
            var favourites = await _mediator.Send(query);

            foreach (var favourite in favourites)
            {
                var command = new RemoveFavouriteCommand(userId, favourite.ProductId);
                await _mediator.Send(command);
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã xóa tất cả sản phẩm yêu thích"));
        }
    }
}
