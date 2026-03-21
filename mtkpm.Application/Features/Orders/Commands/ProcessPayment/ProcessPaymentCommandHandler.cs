using MediatR;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Features.Orders.Commands.ProcessPayment
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, ProcessPaymentResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public ProcessPaymentCommandHandler(
            IPaymentService paymentService,
            IUnitOfWork unitOfWork,
            ILoggerService logger)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ProcessPaymentResponse> Handle(
            ProcessPaymentCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Processing payment for Order {request.OrderId}", "ProcessPaymentHandler");

            try
            {
                // Get order
                var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId, cancellationToken);
                if (order == null)
                {
                    return new ProcessPaymentResponse
                    {
                        Success = false,
                        Message = "Order not found"
                    };
                }

                // Process payment using PaymentService (which uses Factory)
                var paymentResult = await _paymentService.ProcessPaymentAsync(
                    request.PaymentMethod,
                    request.Amount,
                    cancellationToken);

                if (paymentResult.Success)
                {
                    // Mark order as paid
                    order.MarkAsPaid();
                    _unitOfWork.Orders.Update(order);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    _logger.LogInfo($"Order {request.OrderId} marked as paid. Transaction: {paymentResult.TransactionId}", "ProcessPaymentHandler");
                }

                return new ProcessPaymentResponse
                {
                    Success = paymentResult.Success,
                    TransactionId = paymentResult.TransactionId,
                    Message = paymentResult.Message,
                    Status = paymentResult.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Payment processing error for Order {request.OrderId}: {ex.Message}", "ProcessPaymentHandler");
                return new ProcessPaymentResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
