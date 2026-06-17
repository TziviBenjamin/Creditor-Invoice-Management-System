using BL.Api;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "1")]
public class AdminController : ControllerBase
{
    private readonly IUserBl _userBl;
    private readonly IPropertyBl _propertyBl;

    public AdminController(IUserBl userBl, IPropertyBl propertyBl)
    {
        _userBl = userBl;
        _propertyBl = propertyBl;
    }

    [HttpGet("users")]
    public ActionResult<List<User>> GetUsers() => Ok(_userBl.GetAllUsersWithProperties());

    [HttpPost("users")]
    public ActionResult AddUser(User user)
    {
        try
        {
            var id = _userBl.AddUser(user);
            return Ok(new { id });
        }
        catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 2627)
        {
            return Conflict(new { message = $"המשתמש '{user.Username}' כבר קיים במערכת" });
        }
    }

    [HttpGet("properties")]
    public ActionResult<List<Property>> GetProperties() => Ok(_propertyBl.GetAllProperties());

    [HttpPost("assign")]
    public ActionResult AssignProperty([FromQuery] int userId, [FromQuery] int propertyId)
    {
        _propertyBl.AssignPropertyToUser(userId, propertyId);
        return Ok();
    }

    [HttpPost("unassign")]
    public ActionResult UnassignProperty([FromQuery] int userId, [FromQuery] int propertyId)
    {
        _propertyBl.UnassignPropertyFromUser(userId, propertyId);
        return Ok();
    }
}
