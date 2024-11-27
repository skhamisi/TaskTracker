using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Shared.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public double Progress { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public ProjectModel Project { get; set; }
    }
}
