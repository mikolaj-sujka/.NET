using Microsoft.AspNetCore.Mvc;

namespace Limitations.Api.Controllers;

[ApiController]
public class LifetimeController : ControllerBase
{
    [HttpGet("lifetime")]
    public IActionResult Get()
    {
        return Ok();
    }
}
