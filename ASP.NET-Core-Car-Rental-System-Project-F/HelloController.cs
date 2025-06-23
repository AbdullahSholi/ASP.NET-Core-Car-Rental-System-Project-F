using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Car_Rental_System_Project_F;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Hello from API!");
}
