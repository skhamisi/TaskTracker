
namespace TaskTracker.Shared.DTOs
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public UserDto() { }

        public UserDto(UserDto user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Password = user.Password;
            IsAdmin = user.IsAdmin;
        }
    }
}
