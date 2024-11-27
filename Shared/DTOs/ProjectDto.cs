using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Shared.DTOs
{
    public class ProjectDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; } = string.Empty;
        public double? OverallProgress { get; set; }
        public List<TaskDto>? Tasks { get; set; }
    }
}
