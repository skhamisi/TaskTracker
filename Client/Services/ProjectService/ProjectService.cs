
namespace TaskTracker.Client.Services
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/project");
            return response ?? new List<ProjectDto>();
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProjectDto>($"api/project/{id}");
            return response;
        }

        public async Task CreateProjectAsync(ProjectDto projectDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/project/create", projectDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProjectAsync(ProjectDto projectDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/project/update", projectDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task<ServiceResponse<bool>> DeleteProjectAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/project/delete/{id}");
            ServiceResponse<bool> serviceResponse = new();

            if (response.IsSuccessStatusCode) {

                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

                if (result != null)
                {
                    serviceResponse.Success = result.Success;
                    serviceResponse.Message = result.Message;
                }
            }
            return serviceResponse;
        }
    }
}
