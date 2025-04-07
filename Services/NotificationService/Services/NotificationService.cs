using NotificationService.Contracts;
using NotificationService.Model;
using NotificationService.Repository;

namespace NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly INotificationRepository _repository;

        public NotificationService(IEmailSender emailSender, INotificationRepository repository)
        {
            _emailSender = emailSender;
            _repository = repository;
        }

        public async Task HandleOrderPlacedAsync(OrderPlaced message)
        {
            string subject = $"Order #{message.OrderId} Placed!";
            string body = $"Your order was placed on {message.OrderDate}. We'll notify you when it's shipped.";

            await _emailSender.SendEmailAsync(message.CustomerEmail, subject, body);
            await _repository.SaveAsync(new Notification
            {
                Recipient = message.CustomerEmail,
                Subject = subject,
                Message = body
            });
        }

        public async Task HandleOrderStatusChangedAsync(OrderStatusChanged message)
        {
            string subject = $"Order #{message.OrderId} Status Update";
            string body = $"Your order is now: {message.Status} at {message.UpdatedAt}.";

            await _emailSender.SendEmailAsync(message.CustomerEmail, subject, body);
            await _repository.SaveAsync(new Notification
            {
                Recipient = message.CustomerEmail,
                Subject = subject,
                Message = body
            });
        }
    }
}