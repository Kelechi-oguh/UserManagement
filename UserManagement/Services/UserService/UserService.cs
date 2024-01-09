using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;

        public UserService(UserContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            // Hash the password on entry
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;

            _context.Users.Add(user);
            return Save();
        }

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u=> u.Id == userId);
            _context.Users.Remove(user);
            return Save();
        }

        public bool EmailIsValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u=> u.Email == email);

        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(int userId, User request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Name = request.Name;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Username = request.Username;
            user.Password = hashedPassword;
            return Save();
        }

        public bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }
    }
}
