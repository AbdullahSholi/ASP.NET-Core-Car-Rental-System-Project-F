using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    [Authorize(Roles = "User,Admin")]
    [HttpGet("user-data")]
    public IActionResult GetUserData()
    {
        return Ok(new
        {
            Message = "Welcome, User!"
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-data")]
    public IActionResult GetAdminData()
    {
        return Ok(new
        {
            Message = "Welcome, Admin!"
        });
    }
}