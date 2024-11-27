
namespace TaskTracker.Server.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                OverallProgress = CalculateOverallProgress(p),
                Tasks = p.Tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Progress = t.Progress
                }).ToList()
            }).ToList();
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId) 
                ?? throw new Exception($"Project with ID {projectId} not found.");

            return new ProjectDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                OverallProgress = project.OverallProgress,
                Tasks = project.Tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Progress = t.Progress,
                    AssignedToUserId = t.AssignedToUserId,
                    ProjectId = t.ProjectId
                }).ToList()
            };
        }

        public async Task CreateProjectAsync(ProjectDto projectDto)
        {
            var project = new ProjectModel
            {
                Id = projectDto.Id,
                ProjectName = projectDto.ProjectName,
                OverallProgress = 0 
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(ProjectDto projectDto)
        {
            int id = projectDto.Id;
            var project = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id) 
                ?? throw new Exception($"Project with ID {id} not found.");
            project.ProjectName = projectDto.ProjectName;

            if (projectDto.Tasks != null)
            {
                foreach (var taskDto in projectDto.Tasks)
                {
                    var existingTask = project.Tasks.FirstOrDefault(t => t.Id == taskDto.Id);
                    if (existingTask != null)
                    {
                        existingTask.Title = taskDto.Title;
                        existingTask.Description = taskDto.Description;
                        existingTask.IsCompleted = taskDto.IsCompleted;
                        existingTask.Progress = taskDto.Progress;
                    }
                }
            }

            project.OverallProgress = CalculateOverallProgress(project);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceResponse<bool>> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);

            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return new ServiceResponse<bool> { Success = true, Message = "Project Deleted." };
            }
            else
            {
                return new ServiceResponse<bool> { Success = false, Message = "Project could not be deleted." };
            }
        }

        private double CalculateOverallProgress(ProjectModel project)
        {
            if (project.Tasks == null || project.Tasks.Count == 0)
            {
                return 0;
            }

            var totalProgress = project.Tasks.Average(t => t.Progress);
            return totalProgress;
        }
    }
}
