using MediatR;
using mtkpm.Application.Common.DTOs.Notification;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Application.Features.NotificationMethods.Queries.GetNotificationMethods
{
    public class GetNotificationMethodsQuery : IRequest<List<NotificationMethodDto>>
    {
    }

    public class GetNotificationMethodsQueryHandler : IRequestHandler<GetNotificationMethodsQuery, List<NotificationMethodDto>>
    {
        private readonly INotificationMethodService _notificationMethodService;

        public GetNotificationMethodsQueryHandler(INotificationMethodService notificationMethodService)
        {
            _notificationMethodService = notificationMethodService;
        }

        public Task<List<NotificationMethodDto>> Handle(GetNotificationMethodsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_notificationMethodService.GetMethods());
        }
    }
}
