using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SecureController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(new { message = "Este es un endpoint protegido por IdentityServer y JWT." });
    }
}
