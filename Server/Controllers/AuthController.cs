
namespace TaskTracker.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _authService.LoginAsync(loginRequest);

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] UserDto newUser)
        {
            if (!string.IsNullOrEmpty(newUser.Id))
            {
                return BadRequest(new ServiceResponse<string> { Success = false, Message = "Id should not be provided for a new user." });
            }

            var response = await _authService.RegisterAsync(newUser);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _authService.GetUsersAsync();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("users")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userDto)
        {
            var response = await _authService.UpdateUserAsync(userDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Get the currently logged-in user's ID from the claims
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Prevent the current user from deleting themselves
            if (userId == currentUserId)
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = "You cannot delete your own account." });
            }

            var response = await _authService.DeleteUserAsync(userId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
