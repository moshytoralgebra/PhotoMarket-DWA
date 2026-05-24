using Microsoft.AspNetCore.Mvc;
using PhotoMarket.BL.Interfaces;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET api/user
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(_userService.GetAll().Select(u => u.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/user/downloads  — literal route declared before {id} so it takes priority
    [HttpGet("downloads")]
    public IActionResult GetUsersWithDownloads()
    {
        try
        {
            return Ok(_userService.GetUsersWithDownloads().Select(u => u.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/user/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound($"User with id {id} not found.");

            return Ok(user.ToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // PUT api/user/5/profile
    [HttpPut("{id}/profile")]
    public IActionResult UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
    {
        if (request == null)
            return BadRequest("Profile data is required.");

        try
        {
            var success = _userService.UpdateProfile(id, request.FirstName, request.LastName, request.Email, request.PhoneNumber);
            if (!success)
                return NotFound($"User with id {id} not found.");

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
