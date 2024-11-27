
namespace TaskTracker.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/user");
            return response ?? new List<UserDto>();
        }
    }
}

