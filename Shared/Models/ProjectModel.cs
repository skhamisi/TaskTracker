namespace TaskTracker.Shared.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public double OverallProgress { get; set; }
    }
}
