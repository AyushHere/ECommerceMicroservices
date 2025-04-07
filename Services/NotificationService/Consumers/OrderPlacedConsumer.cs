using MassTransit;
using NotificationService.Contracts;
using NotificationService.Services;

namespace NotificationService.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlaced>
    {
        private readonly INotificationService _notificationService;

        public OrderPlacedConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task Consume(ConsumeContext<OrderPlaced> context)
        {
            return _notificationService.HandleOrderPlacedAsync(context.Message);
        }
    }
}
