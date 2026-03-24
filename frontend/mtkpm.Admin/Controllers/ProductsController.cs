using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Product;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Controllers
{
    /// <summary>
    /// Products management controller
    /// </summary>
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ICategoryService categoryService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Display paginated list of products
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? searchTerm = null, int? categoryId = null)
        {
            try
            {
                var result = await _productService.GetProductsAsync(pageIndex, pageSize, categoryId, searchTerm);
                
                // Get categories for filter dropdown
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SelectedCategoryId = categoryId;

                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading products: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading products";
                return View(new Models.PaginatedResponse<ProductViewModel>());
            }
        }

        /// <summary>
        /// Display create product form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(new CreateProductViewModel());
        }

        /// <summary>
        /// Create new product
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            try
            {
                var result = await _productService.CreateProductAsync(model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Product created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to create product";
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                ViewBag.ErrorMessage = "Error creating product";
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }
        }

        /// <summary>
        /// Display product details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading product details: {ex.Message}");
                return NotFound();
            }
        }

        /// <summary>
        /// Display edit product form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                var model = new UpdateProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    CategoryId = product.CategoryId
                };

                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading product edit form: {ex.Message}");
                return NotFound();
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            try
            {
                var result = await _productService.UpdateProductAsync(id, model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Product updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to update product";
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product: {ex.Message}");
                ViewBag.ErrorMessage = "Error updating product";
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }
        }

        /// <summary>
        /// Delete product
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _productService.DeleteProductAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Product deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete product";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting product";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
