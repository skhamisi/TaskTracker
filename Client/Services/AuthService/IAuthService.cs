
namespace TaskTracker.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<bool>> LoginAsync(LoginRequest loginRequest);
        Task<ServiceResponse<bool>> RegisterAsync(UserDto newUser);
        Task LogoutAsync();
        Task<string> GetTokenAsync();
        Task<ServiceResponse<List<UserDto>>> GetUsersAsync();
        Task<ServiceResponse<bool>> UpdateUserAsync(UpdateUserDto userDto);
        Task<ServiceResponse<bool>> DeleteUserAsync(string userId);
        public Task<string> GetUserIdFromTokenAsync();
    }
}
