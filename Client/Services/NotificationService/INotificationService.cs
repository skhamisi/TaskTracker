
namespace TaskTracker.Client.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotificationsByUserIdAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task DeleteNotificationAsync(int notificationId);
    }
}

