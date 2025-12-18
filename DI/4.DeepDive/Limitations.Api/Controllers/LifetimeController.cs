using Limitations.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Limitations.Api.Controllers;

[ApiController]
public class LifetimeController : ControllerBase
{
    private readonly SingletonService _singletonService;
    private readonly TransientService _transientService;
    private readonly ScopedService _scopedService;

    public LifetimeController(SingletonService singletonService, 
        ScopedService scopedService, TransientService transientService)
    {
        _singletonService = singletonService;
        _scopedService = scopedService;
        _transientService = transientService;
    }

    [HttpGet("lifetime")]
    public IActionResult Get()
    {
        var ids = new
        {
            SingletonId = _singletonService.Id,
            ScopedId = _scopedService.Id,
            TransientId = _transientService.Id
        };

        return Ok(ids);
    }
}
