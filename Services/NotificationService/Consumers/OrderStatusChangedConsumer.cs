using MassTransit;
using NotificationService.Contracts;
using NotificationService.Services;

namespace NotificationService.Consumers
{
    public class OrderStatusChangedConsumer : IConsumer<OrderStatusChanged>
    {
        private readonly INotificationService _notificationService;

        public OrderStatusChangedConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task Consume(ConsumeContext<OrderStatusChanged> context)
        {
            return _notificationService.HandleOrderStatusChangedAsync(context.Message);
        }
    }
}