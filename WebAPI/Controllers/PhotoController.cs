using Microsoft.AspNetCore.Mvc;
using PhotoMarket.BL.Interfaces;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoService;

    public PhotoController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    // GET api/photo
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(_photoService.GetAll().Select(p => p.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/photo/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var photo = _photoService.GetById(id);
            if (photo == null)
                return NotFound($"Photo with id {id} not found.");

            return Ok(photo.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/photo/search?searchTerm=sunset&photographerId=2&tagId=3&page=1&pageSize=10
    [HttpGet("search")]
    public IActionResult Search(
        [FromQuery] string? searchTerm,
        [FromQuery] int? photographerId,
        [FromQuery] int? tagId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        try
        {
            var results = _photoService.Search(searchTerm, photographerId, tagId, page, pageSize)
                .Select(p => p.ToDto()).ToList();
            var total = _photoService.CountSearchResults(searchTerm, photographerId, tagId);

            return Ok(new { total, page, pageSize, results });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/photo
    [HttpPost]
    public IActionResult Create([FromBody] PhotoRequest request)
    {
        if (request?.Photo == null)
            return BadRequest("Photo data is required.");

        try
        {
            var photo = _photoService.Create(request.Photo, request.TagIds);
            // Reload to get navigation properties (Photographer, Tags) populated
            var created = _photoService.GetById(photo.Id)!;
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // PUT api/photo/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] PhotoRequest request)
    {
        if (request?.Photo == null)
            return BadRequest("Photo data is required.");

        try
        {
            var success = _photoService.Update(id, request.Photo, request.TagIds);
            if (!success)
                return NotFound($"Photo with id {id} not found.");

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // DELETE api/photo/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var success = _photoService.Delete(id);
            if (!success)
                return NotFound($"Photo with id {id} not found.");

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
