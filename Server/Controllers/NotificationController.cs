
namespace TaskTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetNotificationsByUserId(string userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);

            if (notifications.Count == 0) {

                List<NotificationDto> emptyNotifications= [];
                return Ok(emptyNotifications);
            }

            return Ok(notifications);
        }

        [HttpPut("markasread/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok();
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            await _notificationService.DeleteNotificationAsync(notificationId);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CopyOfNotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public CopyOfNotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetNotificationsByUserId(string userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);

            if (notifications.Count == 0)
            {

                List<NotificationDto> emptyNotifications = [];
                return Ok(emptyNotifications);
            }

            return Ok(notifications);
        }

        [HttpPut("markasread/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok();
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            await _notificationService.DeleteNotificationAsync(notificationId);
            return Ok();
        }
    }
}

