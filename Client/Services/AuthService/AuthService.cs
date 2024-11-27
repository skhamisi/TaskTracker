
namespace TaskTracker.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ServiceResponse<bool>> LoginAsync(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            var serviceResponse = new ServiceResponse<bool>();

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

                if (authResponse.Success)
                {
                    await _localStorageService.SetItemAsync("authToken", authResponse.Data);

                    await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                        .MarkUserAsAuthenticated(authResponse.Data);

                    serviceResponse.Data = true;
                    serviceResponse.Success = true;
                    serviceResponse.Message = authResponse.Message;
                }
                else
                {
                    serviceResponse.Data = false;
                    serviceResponse.Success = false;
                    serviceResponse.Message = authResponse.Message;
                }
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
                serviceResponse.Data = false;
                serviceResponse.Success = false;
                serviceResponse.Message = errorResponse?.Message ?? "An error occurred during login.";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> RegisterAsync(UserDto newUser)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", newUser);

            var serviceResponse = new ServiceResponse<bool>();

            if (response.IsSuccessStatusCode)
            {
                var registerResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

                if (registerResponse.Success)
                {
                    serviceResponse.Data = true;
                    serviceResponse.Success = true;
                    serviceResponse.Message = registerResponse.Message;
                }
                else
                {
                    serviceResponse.Data = false;
                    serviceResponse.Success = false;
                    serviceResponse.Message = registerResponse.Message;
                }
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
                serviceResponse.Data = false;
                serviceResponse.Success = false;
                serviceResponse.Message = errorResponse?.Message ?? "An error occurred during registration.";
            }

            return serviceResponse;
        }

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("authToken");

            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>("authToken");
        }

        public async Task<ServiceResponse<List<UserDto>>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/auth/users");

            var serviceResponse = new ServiceResponse<List<UserDto>>();

            if (response.IsSuccessStatusCode)
            {
                serviceResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<List<UserDto>>>();
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to fetch users.";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateUserAsync(UpdateUserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync("api/auth/users", userDto);

            var serviceResponse = new ServiceResponse<bool>();

            if (response.IsSuccessStatusCode)
            {
                serviceResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to update user.";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/auth/users/{userId}");

            var serviceResponse = new ServiceResponse<bool>();

            if (response.IsSuccessStatusCode)
            {
                serviceResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to delete user.";
            }

            return serviceResponse;
        }

        public async Task<string> GetUserIdFromTokenAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}
