
namespace TaskTracker.Client.Services
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TaskDto>>("api/task");
            return response ?? new List<TaskDto>();
        }

        public async Task<List<TaskDto>> GetTasksByCurrentUserAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TaskDto>>("api/task/user");
            return response ?? new List<TaskDto>();
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<TaskDto>($"api/task/{id}");
            return response;
        }

        public async Task CreateTaskAsync(TaskDto taskDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/task", taskDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTaskAsync(int id, TaskDto taskDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/task/{id}", taskDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task CompleteTaskAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/task/complete/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/task/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
