
namespace TaskTracker.Server.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
    }
}

