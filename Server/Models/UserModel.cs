
namespace TaskTracker.Server.Models
{
    public class UserModel : IdentityUser
    {
        public bool IsAdmin { get; set; }
    }
}

