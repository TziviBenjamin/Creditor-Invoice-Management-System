namespace DAL.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int UserRole { get; set; }
    public List<Property> Properties { get; set; } = new();
}
