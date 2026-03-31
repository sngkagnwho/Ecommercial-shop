using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Discount;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Controllers
{
    [Authorize]
    public class DiscountsController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountsController> _logger;

        public DiscountsController(IDiscountService discountService, ILogger<DiscountsController> logger)
        {
            _discountService = discountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var discounts = await _discountService.GetAllDiscountsAsync();
                return View(discounts ?? new List<DiscountViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading discounts: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading discounts";
                return View(new List<DiscountViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateDiscountViewModel { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDiscountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _discountService.CreateDiscountAsync(model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Discount created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to create discount";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating discount: {ex.Message}");
                ViewBag.ErrorMessage = "Error creating discount";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);
                if (discount == null)
                    return NotFound();

                return View(discount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading discount details: {ex.Message}");
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);
                if (discount == null)
                    return NotFound();

                var model = new UpdateDiscountViewModel
                {
                    Id = discount.Id,
                    Description = discount.Description,
                    EndDate = discount.EndDate,
                    MaxUses = discount.MaxUses,
                    IsActive = discount.IsActive
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading discount edit form: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDiscountViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _discountService.UpdateDiscountAsync(id, model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Discount updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to update discount";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating discount: {ex.Message}");
                ViewBag.ErrorMessage = "Error updating discount";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _discountService.DeleteDiscountAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Discount deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete discount";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting discount: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting discount";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
