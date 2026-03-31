using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Category;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Controllers
{
    /// <summary>
    /// Categories management controller
    /// </summary>
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return View(categories ?? new List<CategoryViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading categories: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading categories";
                return View(new List<CategoryViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _categoryService.CreateCategoryAsync(model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Category created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to create category";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                ViewBag.ErrorMessage = "Error creating category";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound();

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading category details: {ex.Message}");
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound();

                var model = new UpdateCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading category edit form: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCategoryViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _categoryService.UpdateCategoryAsync(id, model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Category updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to update category";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category: {ex.Message}");
                ViewBag.ErrorMessage = "Error updating category";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _categoryService.DeleteCategoryAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Category deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete category";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting category";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
