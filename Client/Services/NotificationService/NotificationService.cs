
namespace TaskTracker.Client.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NotificationDto>> GetNotificationsByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<NotificationDto>>($"api/notification/user/{userId}");
            return response ?? new List<NotificationDto>();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var response = await _httpClient.PutAsync($"api/notification/markasread/{notificationId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            var response = await _httpClient.DeleteAsync($"api/notification/{notificationId}");
            response.EnsureSuccessStatusCode();
        }
    }
}

