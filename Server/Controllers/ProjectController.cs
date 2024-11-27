
namespace TaskTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            return Ok(project);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto)
        {
            await _projectService.CreateProjectAsync(projectDto);
            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDto projectDto)
        {
            await _projectService.UpdateProjectAsync(projectDto);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var response = await _projectService.DeleteProjectAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
