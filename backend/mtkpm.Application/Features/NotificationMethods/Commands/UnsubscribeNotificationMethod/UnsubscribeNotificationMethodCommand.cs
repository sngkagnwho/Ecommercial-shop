using MediatR;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Application.Features.NotificationMethods.Commands.UnsubscribeNotificationMethod
{
    public class UnsubscribeNotificationMethodCommand : IRequest<bool>
    {
        public string MethodName { get; set; } = string.Empty;
    }

    public class UnsubscribeNotificationMethodCommandHandler : IRequestHandler<UnsubscribeNotificationMethodCommand, bool>
    {
        private readonly INotificationMethodService _notificationMethodService;

        public UnsubscribeNotificationMethodCommandHandler(INotificationMethodService notificationMethodService)
        {
            _notificationMethodService = notificationMethodService;
        }

        public Task<bool> Handle(UnsubscribeNotificationMethodCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_notificationMethodService.Unsubscribe(request.MethodName));
        }
    }
}
