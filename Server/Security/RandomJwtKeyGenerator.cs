
namespace TaskTracker.Server.Security
{
    public class RandomJwtKeyGenerator
    {
        public static string GenerateJwtKey(int size = 32)
        {
            // Generate a random JWT Key
            var key = new byte[size];
            RandomNumberGenerator.Fill(key);
            return Convert.ToBase64String(key);
        }
    }
}

