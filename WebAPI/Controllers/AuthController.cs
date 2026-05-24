using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(IAuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
    }

    // POST api/auth/register
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (request == null)
            return BadRequest("Registration data is required.");

        if (string.IsNullOrWhiteSpace(request.UserName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("UserName, Email, and Password are required.");

        try
        {
            var user = _authService.Register(
                request.UserName, request.Email,
                request.FirstName, request.LastName, request.Password);

            if (user == null)
                return BadRequest("Registration failed. The 'User' role may not be seeded in the database.");

            // Never return PasswordHash
            return Created("", new { user.Id, user.UserName, user.Email, user.FirstName, user.LastName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/auth/login
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null)
            return BadRequest("Login data is required.");

        try
        {
            var user = _authService.ValidateLogin(request.UserName, request.Password);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                userId   = user.Id,
                userName = user.UserName,
                role     = user.Role?.Name
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/auth/changepassword
    [HttpPost("changepassword")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (request == null)
            return BadRequest("Request data is required.");

        try
        {
            var success = _authService.ChangePassword(
                request.UserId, request.CurrentPassword, request.NewPassword);

            if (!success)
                return BadRequest("Password change failed. Check your current password.");

            return Ok("Password changed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Builds and signs a JWT token for the authenticated user
    private string GenerateJwtToken(AppUser user)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpirationMinutes"]));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
        };

        var token = new JwtSecurityToken(
            issuer:            _config["Jwt:Issuer"],
            audience:          _config["Jwt:Audience"],
            claims:            claims,
            expires:           expiry,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
