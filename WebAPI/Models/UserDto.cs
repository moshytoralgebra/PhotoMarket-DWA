namespace WebAPI.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? RoleName { get; set; }
}
