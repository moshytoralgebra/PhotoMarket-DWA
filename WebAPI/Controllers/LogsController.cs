using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoMarket.BL.Interfaces;
using WebAPI.Mapping;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/logs")]
public class LogsController : ControllerBase
{
    private readonly ILogService _logService;

    public LogsController(ILogService logService)
    {
        _logService = logService;
    }

    // GET api/logs/get/50  — requires valid Bearer token
    [HttpGet("get/{count}")]
    public IActionResult GetLast(int count)
    {
        if (count <= 0)
            return BadRequest("Count must be greater than 0.");

        try
        {
            return Ok(_logService.GetLast(count).Select(l => l.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/logs/count  — requires valid Bearer token
    [HttpGet("count")]
    public IActionResult Count()
    {
        try
        {
            return Ok(_logService.Count());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
