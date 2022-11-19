using Microsoft.AspNetCore.Mvc;
namespace ApiToDatabase.Controllers;

[ApiController]
[Route("api")]
public class EmployeeController : ControllerBase
{

    [HttpPost("userinfo")]
    public void SendToDatabase()
    {
        
    }
}