namespace WebAPI.Models;

public class RegisterRequest
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class ChangePasswordRequest
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
