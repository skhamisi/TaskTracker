
namespace TaskTracker.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<List<TaskDto>> GetTasksByUserIdAsync(string userId)
        {
            var tasks = await _context.Tasks.Where(t => t.AssignedToUserId == userId).ToListAsync();
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            return task == null ? null : MapToDto(task);
        }

        public async Task CreateTaskAsync(TaskDto taskDto)
        {
            var task = new TaskModel
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                Progress = taskDto.Progress,
                AssignedToUserId = taskDto.AssignedToUserId,
                AssignedToUserName = taskDto.AssignedToUserName,
                ProjectId = taskDto.ProjectId,
                AssignedDate = DateTime.Now
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await NotifyUserAsync(task.AssignedToUserId, task.AssignedToUserName, $"You have been assigned a new task: '{task.Title}'.");
        }

        public async Task UpdateTaskAsync(int id, TaskDto taskDto)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return;

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.Progress = taskDto.Progress;
            task.IsCompleted = taskDto.IsCompleted;
            task.AssignedToUserId = taskDto.AssignedToUserId;
            task.ProjectId = taskDto.ProjectId;

            await _context.SaveChangesAsync();
        }

        public async Task CompleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return;

            task.IsCompleted = true;
            task.Progress = 100;
            task.CompletedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            await NotifyAdminsAsync($"Task '{task.Title}' has been completed by '{task.AssignedToUserName}'.");

            await CheckProjectCompletionAsync(task.ProjectId);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        private TaskDto MapToDto(TaskModel task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                Progress = task.Progress,
                AssignedToUserId = task.AssignedToUserId,
                ProjectId = task.ProjectId
            };
        }

        private async Task NotifyAdminsAsync(string message)
        {
            var adminRoleId = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin"))?.Id;
            if (adminRoleId != null)
            {
                var adminUserIds = await _context.UserRoles
                    .Where(ur => ur.RoleId == adminRoleId)
                    .Select(ur => ur.UserId)
                    .ToListAsync();

                foreach (var adminUserId in adminUserIds)
                {
                    var notification = new NotificationModel
                    {
                        Message = message,
                        IsRead = false,
                        CreatedAt = DateTime.Now,
                        UserId = adminUserId
                    };
                    _context.Notifications.Add(notification);
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProjectCompletionAsync(int? projectId)
        {
            if (projectId == null) return;

            var project = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == projectId);
            if (project != null)
            {
                var allTasksCompleted = project.Tasks.All(t => t.IsCompleted);
                if (allTasksCompleted)
                {
                    project.OverallProgress = 100;
                    await _context.SaveChangesAsync();

                    await NotifyAdminsAsync($"Project '{project.ProjectName}' has been completed.");
                }
            }
        }

        private async Task NotifyUserAsync(string userId, string userName, string message)
        {
            var notification = new NotificationModel
            {
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
                UserId = userId
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}
