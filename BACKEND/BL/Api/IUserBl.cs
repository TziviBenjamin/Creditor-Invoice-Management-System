using DAL.Models;

namespace BL.Api;

public interface IUserBl
{
    int AddUser(User user);
    List<User> GetAllUsers();
    List<User> GetAllUsersWithProperties();
}
