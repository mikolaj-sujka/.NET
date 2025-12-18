using Microsoft.AspNetCore.Mvc;
using Weather.Api.Filter;
using Weather.Api.Service;

namespace Weather.Api.Controllers;

[ApiController]
public class LifeTimeController : ControllerBase
{
    private readonly IdGenerator _idGenerator;
    public LifeTimeController(IdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    [HttpGet("lifetime")]
    [ServiceFilter(typeof(LifetimeIndicatorFilter))]
    public IActionResult Get()
    {
        return Ok(new { Id = _idGenerator.Id });
    }
}