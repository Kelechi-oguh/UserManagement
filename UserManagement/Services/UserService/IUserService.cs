using UserManagement.Models;

namespace UserManagement.Services.UserService
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        bool UserExists(int userId);
        User GetUserById(int userId);
        User GetUserByEmail(string userName);
        bool CreateUser(User user);
        bool UpdateUser(int userId, User request);
        bool DeleteUser(int userId);
        bool Save();
        bool EmailIsValid(string email);
    }
}
