using TaskTracker.Shared.DTOs;
using TaskTracker.Shared.Models;

namespace TaskTracker.Server.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> LoginAsync(LoginRequest loginRequest);
        Task<ServiceResponse<string>> RegisterAsync(UserDto newUser);
        Task<ServiceResponse<List<UserDto>>> GetUsersAsync();
        Task<ServiceResponse<bool>> UpdateUserAsync(UpdateUserDto userDto);
        Task<ServiceResponse<bool>> DeleteUserAsync(string userId);
    }
}
