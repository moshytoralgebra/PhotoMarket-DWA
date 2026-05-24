using Microsoft.AspNetCore.Mvc;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;
using WebAPI.Mapping;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // GET api/tag
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(_tagService.GetAll().Select(t => t.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/tag/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var tag = _tagService.GetById(id);
            if (tag == null)
                return NotFound($"Tag with id {id} not found.");

            return Ok(tag.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/tag
    [HttpPost]
    public IActionResult Create([FromBody] Tag tag)
    {
        if (tag == null)
            return BadRequest("Tag data is required.");

        try
        {
            var created = _tagService.Create(tag);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // PUT api/tag/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Tag tag)
    {
        if (tag == null)
            return BadRequest("Tag data is required.");

        try
        {
            var success = _tagService.Update(id, tag);
            if (!success)
                return NotFound($"Tag with id {id} not found.");

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // DELETE api/tag/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var success = _tagService.Delete(id);
            if (!success)
                return NotFound($"Tag with id {id} not found.");

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
