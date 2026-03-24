using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var paymentHistory = await _paymentService.GetPaymentHistoryAsync();
                return View(paymentHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading payment history: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading payment history";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                    return NotFound();

                return View(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading payment details: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refund(int id, decimal refundAmount, string reason)
        {
            try
            {
                var success = await _paymentService.RefundPaymentAsync(id, refundAmount, reason);
                if (success)
                {
                    TempData["SuccessMessage"] = "Refund processed successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to process refund";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing refund: {ex.Message}");
                TempData["ErrorMessage"] = "Error processing refund";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
