using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Model;

namespace NotificationService.Repository
{
    public interface INotificationRepository
    {
        Task SaveAsync(Notification notification);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }
        public async Task SaveAsync(Notification notification)
        {
            Console.WriteLine($"[Saved] To: {notification.Recipient}, Subject: {notification.Subject}");
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}
