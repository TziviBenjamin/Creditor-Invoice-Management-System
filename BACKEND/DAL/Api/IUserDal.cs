using DAL.Models;

namespace DAL.Api;

public interface IUserDal
{
    int AddUser(User user);
    List<User> GetAllUsers();
    List<User> GetAllUsersWithProperties();
}
