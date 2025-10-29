using ProductApi.Models;

namespace ProductApi.Services
{
    public class AuthService
    {
        // ตัวอย่าง: mock user สำหรับทดสอบ
        private readonly List<User> _users = new()
        {
            new User { Username = "admin", Password = "password" },
        };

        public User? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
