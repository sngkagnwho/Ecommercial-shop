using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Product;
using mtkpm.Application.Features.Products.Commands.CreateProduct;
using mtkpm.Application.Features.Products.Commands.DeleteProduct;
using mtkpm.Application.Features.Products.Commands.UpdateProduct;
using mtkpm.Application.Features.Products.Commands.UpdateStock;
using mtkpm.Application.Features.Products.Queries.GetAllProducts;
using mtkpm.Application.Features.Products.Queries.GetProductById;
using mtkpm.Application.Features.Products.Queries.GetProductsByCategory;
using mtkpm.Application.Features.Products.Queries.GetProductsPaginated;
using mtkpm.Application.Features.Products.Queries.SearchProducts;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm (phân trang)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedListDto<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] int? categoryId = null, [FromQuery] string? searchTerm = null)
        {
            var query = new GetProductsPaginatedQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId,
                SearchTerm = searchTerm
            };

            var result = await _mediator.Send(query);
            return Ok(ApiResponse<PaginatedListDto<ProductDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy tất cả sản phẩm
        /// </summary>
        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(ApiResponse<ProductDto>.FailureResponse("Không tìm thấy sản phẩm"));
            }

            return Ok(ApiResponse<ProductDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy sản phẩm theo danh mục
        /// </summary>
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var query = new GetProductsByCategoryQuery(categoryId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Tìm kiếm sản phẩm
        /// </summary>
        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm)
        {
            var query = new SearchProductsQuery(searchTerm);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Tạo sản phẩm mới (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var command = new CreateProductCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, ApiResponse<ProductDto>.SuccessResponse(result, "Tạo sản phẩm thành công"));
        }

        /// <summary>
        /// Cập nhật sản phẩm (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var command = new UpdateProductCommand
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<ProductDto>.SuccessResponse(result, "Cập nhật sản phẩm thành công"));
        }

        /// <summary>
        /// Xóa sản phẩm (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Xóa sản phẩm thành công"));
        }

        /// <summary>
        /// Cập nhật số lượng tồn kho (Admin only)
        /// </summary>
        [HttpPatch("{id}/stock")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
        {
            var command = new UpdateStockCommand(id, quantity);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Cập nhật tồn kho thành công"));
        }
    }
}
