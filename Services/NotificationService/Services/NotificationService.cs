using NotificationService.Repository;

namespace NotificationService.Services
{
    public interface INotificationService
    {
        Task SendNotification(string message);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task SendNotification(string message)
        {
            Console.WriteLine($"Sending Notification: {message}");
            await _repository.SaveNotification(message);
        }
    }
}