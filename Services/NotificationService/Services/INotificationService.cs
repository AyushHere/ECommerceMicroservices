using NotificationService.Contracts;

namespace NotificationService.Services
{
    public interface INotificationService
    {
        Task HandleOrderPlacedAsync(OrderPlaced message);
        Task HandleOrderStatusChangedAsync(OrderStatusChanged message);
    }
}
