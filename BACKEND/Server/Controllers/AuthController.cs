using DAL.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserDal _userDal;
    private readonly IConfiguration _config;

    public AuthController(IUserDal userDal, IConfiguration config)
    {
        _userDal = userDal;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var users = _userDal.GetAllUsers();
        var user = users.FirstOrDefault(u =>
            u.Username == request.Username && u.Password == request.Password);

        if (user == null)
            return Unauthorized(new { message = "שם משתמש או סיסמה שגויים" });

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            claims: [
                new Claim("userId", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            ],
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            username = user.Username,
            role = user.UserRole
        });
    }
}

public record LoginRequest(string Username, string Password);
