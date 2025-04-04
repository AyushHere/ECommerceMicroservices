using NotificationService.Repository;

namespace NotificationService.Services
{
    public interface INotificationService
    {
        Task SendNotification(string message);
    }

    public class NotificationService : INotificationService
    {
        public Task SendNotification(string message)
        {
            Console.WriteLine($"[NotificationService] Sending notification: {message}");
            return Task.CompletedTask;
        }
    }
}