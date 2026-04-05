using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Discount;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Discounts.Controllers
{
    [Authorize]
    public class DiscountsController : Controller
    {
        private readonly IAdminDiscountService _discountService;
        private readonly ILogger<DiscountsController> _logger;

        public DiscountsController(IAdminDiscountService discountService, ILogger<DiscountsController> logger)
        {
            _discountService = discountService;
            _logger = logger;
        }

        /// <summary>
        /// Display discount management dashboard with calculator and CRUD list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var discounts = await _discountService.GetDiscountsAsync(includeInactive: false);
                var availableDiscounts = await _discountService.GetAvailableDiscountsAsync();
                var stats = await _discountService.GetDiscountStatisticsAsync();

                ViewBag.Discounts = discounts ?? new List<DiscountViewModel>();
                ViewBag.AvailableDiscounts = availableDiscounts ?? new List<DiscountCodeDto>();
                ViewBag.Statistics = stats ?? new DiscountStatisticsViewModel();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading discount console: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading discount console";
                return View();
            }
        }

        /// <summary>
        /// Calculate discount for test/demo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] TestDiscountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (request.DiscountCodes == null || request.DiscountCodes.Count == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select at least one discount code"
                    });
                }

                var result = await _discountService.CalculateDiscountAsync(request.DiscountCodes);

                if (result == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Failed to calculate discount"
                    });
                }

                return Json(new
                {
                    success = true,
                    data = result,
                    message = "Calculation successful"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating discount: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = "Error calculating discount"
                });
            }
        }

        /// <summary>
        /// Create new discount form
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateDiscountViewModel());
        }

        /// <summary>
        /// Create new discount
        /// </summary>
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

        /// <summary>
        /// Edit discount form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);
                if (discount == null)
                {
                    TempData["ErrorMessage"] = "Discount not found";
                    return RedirectToAction(nameof(Index));
                }

                var model = new UpdateDiscountViewModel
                {
                    Name = discount.Name,
                    Description = discount.Description,
                    DiscountType = discount.DiscountType,
                    DiscountValue = discount.DiscountValue,
                    MinimumOrderAmount = discount.MinimumOrderAmount,
                    MaximumDiscountAmount = discount.MaximumDiscountAmount,
                    StartDate = discount.StartDate,
                    EndDate = discount.EndDate,
                    MaxUsageCount = discount.MaxUsageCount,
                    MaxUsagePerUser = discount.MaxUsagePerUser,
                    BudgetLimit = discount.BudgetLimit,
                    IsActive = discount.IsActive,
                    IsNewUserOnly = discount.IsNewUserOnly,
                    IsStackable = discount.IsStackable
                };

                ViewBag.DiscountId = id;
                ViewBag.DiscountCode = discount.Code;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading discount {id}: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading discount";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Update discount
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDiscountViewModel model)
        {
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
                ViewBag.DiscountId = id;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating discount {id}: {ex.Message}");
                ViewBag.ErrorMessage = "Error updating discount";
                ViewBag.DiscountId = id;
                return View(model);
            }
        }

        /// <summary>
        /// Delete discount
        /// </summary>
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
                _logger.LogError($"Error deleting discount {id}: {ex.Message}");
                TempData["ErrorMessage"] = "Error deleting discount";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
