using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Category;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Features.Categories.Commands.CreateCategory;
using mtkpm.Application.Features.Categories.Commands.DeleteCategory;
using mtkpm.Application.Features.Categories.Commands.UpdateCategory;
using mtkpm.Application.Features.Categories.Queries.GetAllCategories;
using mtkpm.Application.Features.Categories.Queries.GetCategoryById;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy tất cả danh mục
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy danh mục theo ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(ApiResponse<CategoryDto>.FailureResponse("Không tìm thấy danh mục"));
            }

            return Ok(ApiResponse<CategoryDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Tạo danh mục mới (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var command = new CreateCategoryCommand
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, ApiResponse<CategoryDto>.SuccessResponse(result, "Tạo danh mục thành công"));
        }

        /// <summary>
        /// Cập nhật danh mục (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var command = new UpdateCategoryCommand
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description
            };

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<CategoryDto>.SuccessResponse(result, "Cập nhật danh mục thành công"));
        }

        /// <summary>
        /// Xóa danh mục (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Xóa danh mục thành công"));
        }
    }
}
