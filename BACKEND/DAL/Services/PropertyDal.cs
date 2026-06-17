using DAL.Api;
using DAL.Models;
using System.Data.SqlClient;

namespace DAL.Services;

public class PropertyDal : IPropertyDal
{
    private readonly string _connectionString;

    public PropertyDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Property> GetAllProperties()
    {
        var properties = new List<Property>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand("SELECT PropertyID, PropertyName FROM Properties", connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            properties.Add(new Property
            {
                Id = (int)reader["PropertyID"],
                Name = (string)reader["PropertyName"]
            });
        return properties;
    }

    public List<Property> GetPropertiesByUser(int userId)
    {
        var properties = new List<Property>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand(
            "SELECT p.PropertyID, p.PropertyName FROM Properties p JOIN UserProperties up ON p.PropertyID = up.PropertyID WHERE up.UserID = @UserId",
            connection);
        cmd.Parameters.AddWithValue("@UserId", userId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            properties.Add(new Property
            {
                Id = (int)reader["PropertyID"],
                Name = (string)reader["PropertyName"]
            });
        return properties;
    }

    public void AssignPropertyToUser(int userId, int propertyId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand(
            "INSERT INTO UserProperties (UserID, PropertyID) VALUES (@UserId, @PropertyId)",
            connection);
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@PropertyId", propertyId);
        cmd.ExecuteNonQuery();
    }

    public void UnassignPropertyFromUser(int userId, int propertyId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand(
            "DELETE FROM UserProperties WHERE UserID = @UserId AND PropertyID = @PropertyId",
            connection);
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@PropertyId", propertyId);
        cmd.ExecuteNonQuery();
    }
}
