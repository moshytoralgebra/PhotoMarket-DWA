using Microsoft.AspNetCore.Mvc;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;
using WebAPI.Mapping;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotographerController : ControllerBase
{
    private readonly IPhotographerService _photographerService;

    public PhotographerController(IPhotographerService photographerService)
    {
        _photographerService = photographerService;
    }

    // GET api/photographer
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(_photographerService.GetAll().Select(p => p.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/photographer/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var photographer = _photographerService.GetById(id);
            if (photographer == null)
                return NotFound($"Photographer with id {id} not found.");

            return Ok(photographer.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/photographer
    [HttpPost]
    public IActionResult Create([FromBody] Photographer photographer)
    {
        if (photographer == null)
            return BadRequest("Photographer data is required.");

        try
        {
            var created = _photographerService.Create(photographer);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // PUT api/photographer/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Photographer photographer)
    {
        if (photographer == null)
            return BadRequest("Photographer data is required.");

        try
        {
            var success = _photographerService.Update(id, photographer);
            if (!success)
                return NotFound($"Photographer with id {id} not found.");

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // DELETE api/photographer/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var photographer = _photographerService.GetById(id);
            if (photographer == null)
                return NotFound($"Photographer with id {id} not found.");

            var success = _photographerService.Delete(id);
            if (!success)
                return BadRequest("Cannot delete photographer because they still have photos assigned.");

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
