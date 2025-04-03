using MassTransit.SqlTransport;
using NotificationService.Model;

namespace NotificationService.Repository
{
    public interface INotificationRepository
    {
        Task SaveNotification(string message);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationContext _context;

        public NotificationRepository(NotificationContext context)
        {
            _context = context;
        }

        public async Task SaveNotification(string message)
        {
            var notification = new Notification
            {
                Recipient = "user@example.com",
                Message = message
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}
