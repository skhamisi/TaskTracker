
namespace TaskTracker.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<List<UserDto>>> GetUsersAsync()
        {
            var response = new ServiceResponse<List<UserDto>>();

            // Load all users into memory
            var usersList = await _userManager.Users.ToListAsync();

            var usersDtoList = new List<UserDto>();

            foreach (var user in usersList)
            {
                // Check if the user is in the "Admin" role
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                // Add the user to the DTO list
                usersDtoList.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    IsAdmin = isAdmin
                });
            }

            response.Data = usersDtoList;
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateUserAsync(UpdateUserDto userDto)
        {
            var response = new ServiceResponse<bool>();

            var user = await _userManager.FindByIdAsync(userDto.Id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            user.UserName = userDto.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = string.Join("; ", result.Errors.Select(e => e.Description));
                return response;
            }

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, userDto.Password);

                if (!passwordResult.Succeeded)
                {
                    response.Success = false;
                    response.Message = string.Join("; ", passwordResult.Errors.Select(e => e.Description));
                    return response;
                }
            }

            // Update roles
            var roles = await _userManager.GetRolesAsync(user);

            bool isAdmin = userDto.IsAdmin ?? false;

            if (isAdmin && !roles.Contains("Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else if (!isAdmin && roles.Contains("Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }

            response.Data = true;
            response.Message = "User updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(string userId)
        {
            var response = new ServiceResponse<bool>();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = string.Join("; ", result.Errors.Select(e => e.Description));
                return response;
            }

            response.Data = true;
            response.Message = "User deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<string>> RegisterAsync(UserDto newUser)
        {
            var response = new ServiceResponse<string>();

            var userExists = await _userManager.FindByNameAsync(newUser.UserName.ToLowerInvariant());
            if (userExists != null)
            {
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }

            var user = new UserModel
            {
                UserName = newUser.UserName
            };

            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = string.Join("; ", result.Errors.Select(e => e.Description));
                return response;
            }

            if (newUser.IsAdmin)
            {
                // Ensure the "Admin" role exists
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await _userManager.AddToRoleAsync(user, "Admin");
            }

            response.Success = true;
            response.Message = "User registered successfully";
            return response;
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginRequest loginRequest)
        {
            var response = new ServiceResponse<string>();

            var username = loginRequest.Username.Trim().ToLowerInvariant();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid credentials";
                return response;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (!isPasswordValid)
            {
                response.Success = false;
                response.Message = "Invalid credentials";
                return response;
            }

            var token = await GenerateJwtToken(user);
            response.Data = token;
            response.Success = true;
            response.Message = "Login successful";
            return response;
        }

        private async Task<string> GenerateJwtToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
