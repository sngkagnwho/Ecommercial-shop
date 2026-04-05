using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Models.Payment;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Payments.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IAdminPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IAdminPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Display list of payment methods with search and statistics
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm = null)
        {
            try
            {
                // Get statistics
                var stats = await _paymentService.GetPaymentStatisticsAsync();
                ViewBag.Statistics = stats ?? new PaymentStatisticsViewModel();

                List<PaymentMethodViewModel>? methods;

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    methods = await _paymentService.SearchPaymentMethodsAsync(searchTerm);
                    ViewBag.SearchTerm = searchTerm;
                }
                else
                {
                    methods = await _paymentService.GetPaymentMethodsAsync();
                }

                return View(methods ?? new List<PaymentMethodViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading payment methods: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading payment methods";
                return View(new List<PaymentMethodViewModel>());
            }
        }

        /// <summary>
        /// Display create payment method form
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreatePaymentMethodViewModel
            {
                DisplayOrder = 0,
                MinAmount = 0,
                MaxAmount = 9999999999,
                ProcessingTime = "Tức thì"
            };
            return View(model);
        }

        /// <summary>
        /// Create new payment method
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentMethodViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _paymentService.CreatePaymentMethodAsync(model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = $"Payment method '{result.Name}' created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to create payment method";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating payment method: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Display payment method details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var methods = await _paymentService.GetPaymentMethodsAsync();
                var method = methods.FirstOrDefault(m => m.Id == id);

                if (method == null)
                {
                    TempData["ErrorMessage"] = "Payment method not found";
                    return RedirectToAction(nameof(Index));
                }

                return View(method);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading payment method details: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading payment method details";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Display edit payment method form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var methods = await _paymentService.GetPaymentMethodsAsync();
                var method = methods.FirstOrDefault(m => m.Id == id);

                if (method == null)
                {
                    TempData["ErrorMessage"] = "Payment method not found";
                    return RedirectToAction(nameof(Index));
                }

                var model = new UpdatePaymentMethodViewModel
                {
                    Id = method.Id,
                    Code = method.Code,
                    Name = method.Name,
                    Description = method.Description,
                    Icon = method.Icon,
                    IsActive = method.IsActive,
                    DisplayOrder = method.DisplayOrder,
                    TransactionFeePercentage = method.TransactionFeePercentage,
                    TransactionFeeFixed = method.TransactionFeeFixed,
                    MinAmount = method.MinAmount,
                    MaxAmount = method.MaxAmount,
                    ProcessingTime = method.ProcessingTime,
                    Requirements = method.Requirements,
                    SupportedProviders = method.SupportedProviders,
                    SupportedAreas = method.SupportedAreas,
                    AdminNotes = method.AdminNotes
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading payment method edit form: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading payment method for edit";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Update payment method
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePaymentMethodViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _paymentService.UpdatePaymentMethodAsync(id, model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Payment method updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "Failed to update payment method";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating payment method {id}: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Delete payment method
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _paymentService.DeletePaymentMethodAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Payment method deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete payment method";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting payment method {id}: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Toggle payment method active status
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id, bool isActive)
        {
            try
            {
                var methods = await _paymentService.GetPaymentMethodsAsync();
                var method = methods.FirstOrDefault(m => m.Id == id);

                if (method == null)
                {
                    TempData["ErrorMessage"] = "Payment method not found";
                    return RedirectToAction(nameof(Index));
                }

                var updateModel = new UpdatePaymentMethodViewModel
                {
                    Name = method.Name,
                    Description = method.Description,
                    Icon = method.Icon,
                    IsActive = isActive,
                    DisplayOrder = method.DisplayOrder,
                    TransactionFeePercentage = method.TransactionFeePercentage,
                    TransactionFeeFixed = method.TransactionFeeFixed,
                    MinAmount = method.MinAmount,
                    MaxAmount = method.MaxAmount,
                    ProcessingTime = method.ProcessingTime,
                    Requirements = method.Requirements,
                    SupportedProviders = method.SupportedProviders,
                    SupportedAreas = method.SupportedAreas,
                    AdminNotes = method.AdminNotes
                };

                var result = await _paymentService.UpdatePaymentMethodAsync(id, updateModel);
                if (result != null)
                {
                    TempData["SuccessMessage"] = $"Payment method {(isActive ? "activated" : "deactivated")} successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to toggle payment method status";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error toggling payment method status {id}: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
