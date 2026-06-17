using BL.Api;
using DAL.Api;
using DAL.Models;

namespace BL.Services;

public class UserBl : IUserBl
{
    private readonly IUserDal _userDal;

    public UserBl(IUserDal userDal)
    {
        _userDal = userDal;
    }

    public int AddUser(User user) => _userDal.AddUser(user);

    public List<User> GetAllUsers() => _userDal.GetAllUsers();

    public List<User> GetAllUsersWithProperties() => _userDal.GetAllUsersWithProperties();
}
