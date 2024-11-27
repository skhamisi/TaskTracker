namespace TaskTracker.Server.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto> GetProjectByIdAsync(int id);
        Task CreateProjectAsync(ProjectDto projectDto);
        Task UpdateProjectAsync(ProjectDto projectDto);
        Task<ServiceResponse<bool>> DeleteProjectAsync(int id);
    }
}
