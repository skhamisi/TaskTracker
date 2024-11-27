using TaskTracker.Shared.DTOs;

namespace TaskTracker.Server.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<List<TaskDto>> GetTasksByUserIdAsync(string userId);
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(TaskDto taskDto);
        Task UpdateTaskAsync(int id, TaskDto taskDto);
        Task CompleteTaskAsync(int id);
        Task DeleteTaskAsync(int id);
    }
}
