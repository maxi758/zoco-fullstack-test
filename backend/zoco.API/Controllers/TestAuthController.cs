using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace zoco.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestAuthController : ControllerBase
{
    [Authorize]
    [HttpGet("secure")]
    public IActionResult Secure()
    {
        return Ok("JWT funcionando");
    }
}