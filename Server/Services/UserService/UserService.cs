
namespace TaskTracker.Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserModel> _userManager;

        public UserService(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName
            }).ToList();
        }
    }
}

