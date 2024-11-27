namespace TaskTracker.Shared.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public double Progress { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public int? ProjectId { get; set; }
    }
}
