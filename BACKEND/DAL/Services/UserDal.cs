using DAL.Api;
using DAL.Models;
using System.Data.SqlClient;

namespace DAL.Services;

public class UserDal : IUserDal
{
    private readonly string _connectionString;

    public UserDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddUser(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand(
            "INSERT INTO Users (Username, PasswordHash, UserRole) OUTPUT INSERTED.UserId VALUES (@Username, @PasswordHash, @UserRole)",
            connection);
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@PasswordHash", user.Password);
        cmd.Parameters.AddWithValue("@UserRole", user.UserRole);
        return (int)cmd.ExecuteScalar();
    }

    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand("SELECT UserId, Username, PasswordHash, UserRole FROM Users", connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            users.Add(new User
            {
                Id = (int)reader["UserId"],
                Username = (string)reader["Username"],
                Password = (string)reader["PasswordHash"],
                UserRole = reader["UserRole"] is int role ? role : Convert.ToInt32(reader["UserRole"])
            });
        return users;
    }

    public List<User> GetAllUsersWithProperties()
    {
        var users = new Dictionary<int, User>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand(
            "SELECT u.UserId, u.Username, u.PasswordHash, u.UserRole, p.PropertyID, p.PropertyName " +
            "FROM Users u " +
            "LEFT JOIN UserProperties up ON up.UserID = u.UserId " +
            "LEFT JOIN Properties p ON p.PropertyID = up.PropertyID " +
            "ORDER BY u.UserId", connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var userId = (int)reader["UserId"];
            if (!users.TryGetValue(userId, out var user))
            {
                user = new User
                {
                    Id = userId,
                    Username = (string)reader["Username"],
                    Password = (string)reader["PasswordHash"],
                    UserRole = reader["UserRole"] is int role ? role : Convert.ToInt32(reader["UserRole"])
                };
                users[userId] = user;
            }

            if (reader["PropertyID"] != DBNull.Value)
            {
                user.Properties.Add(new Property
                {
                    Id = (int)reader["PropertyID"],
                    Name = (string)reader["PropertyName"]
                });
            }
        }
        return users.Values.ToList();
    }
}
