using MediatR;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Application.Features.NotificationMethods.Commands.SubscribeNotificationMethod
{
    public class SubscribeNotificationMethodCommand : IRequest<bool>
    {
        public string MethodName { get; set; } = string.Empty;
    }

    public class SubscribeNotificationMethodCommandHandler : IRequestHandler<SubscribeNotificationMethodCommand, bool>
    {
        private readonly INotificationMethodService _notificationMethodService;

        public SubscribeNotificationMethodCommandHandler(INotificationMethodService notificationMethodService)
        {
            _notificationMethodService = notificationMethodService;
        }

        public Task<bool> Handle(SubscribeNotificationMethodCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_notificationMethodService.Subscribe(request.MethodName));
        }
    }
}
