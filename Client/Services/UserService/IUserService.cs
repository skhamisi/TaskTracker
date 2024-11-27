
namespace TaskTracker.Client.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
    }
}
