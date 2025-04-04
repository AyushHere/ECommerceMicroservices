using MassTransit;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Messaging
{
    public class OrderNotificationConsumer : IConsumer<OrderNotificationContract>
    {
        private readonly INotificationService _notificationService;

        public OrderNotificationConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<OrderNotificationContract> context)
        {
            var message = context.Message;
            Console.WriteLine($"Received notification for Order ID: {message.OrderId}");

            await _notificationService.SendNotification($"{message.Message} (Order ID: {message.OrderId})");
        }
    }
}