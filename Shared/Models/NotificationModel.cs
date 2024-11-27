namespace TaskTracker.Server.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public string? UserName { get; set; } = string.Empty;
    }
}

