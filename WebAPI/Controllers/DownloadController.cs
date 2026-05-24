using Microsoft.AspNetCore.Mvc;
using PhotoMarket.BL.Interfaces;
using WebAPI.Mapping;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DownloadController : ControllerBase
{
    private readonly IDownloadService _downloadService;

    public DownloadController(IDownloadService downloadService)
    {
        _downloadService = downloadService;
    }

    // GET api/download
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(_downloadService.GetAllWithDetails().Select(d => d.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/download/user/3
    [HttpGet("user/{userId}")]
    public IActionResult GetByUser(int userId)
    {
        if (userId <= 0)
            return BadRequest("Invalid user id.");

        try
        {
            return Ok(_downloadService.GetByUser(userId).Select(d => d.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/download/photo/7
    [HttpGet("photo/{photoId}")]
    public IActionResult GetByPhoto(int photoId)
    {
        if (photoId <= 0)
            return BadRequest("Invalid photo id.");

        try
        {
            return Ok(_downloadService.GetByPhoto(photoId).Select(d => d.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/download/photo/7?userId=3
    [HttpPost("photo/{photoId}")]
    public IActionResult Create(int photoId, [FromQuery] int userId)
    {
        if (userId <= 0 || photoId <= 0)
            return BadRequest("userId and photoId must be greater than 0.");

        try
        {
            var download = _downloadService.Create(userId, photoId);
            return Ok(download.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
