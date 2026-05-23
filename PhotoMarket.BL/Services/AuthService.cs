using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Services;

// NOTE: Uses SHA256 for password hashing — simple enough for a student project.
// For a real app, replace with BCrypt or ASP.NET Core Identity.
public class AuthService : IAuthService
{
    private readonly PhotoMarketContext _context;

    public AuthService(PhotoMarketContext context)
    {
        _context = context;
    }

    public AppUser? Register(string userName, string email, string firstName, string lastName, string password)
    {
        // Resolve the default "User" role from the database
        var userRole = _context.Roles.FirstOrDefault(r => r.Name == "User");
        if (userRole == null) return null;

        var user = new AppUser
        {
            UserName = userName,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Name = $"{firstName} {lastName}",
            PasswordHash = HashPassword(password),
            RoleId = userRole.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.AppUsers.Add(user);
        _context.SaveChanges();
        return user;
    }

    public AppUser? ValidateLogin(string userName, string password)
    {
        var user = _context.AppUsers
            .Include(u => u.Role)
            .FirstOrDefault(u => u.UserName == userName);

        if (user == null) return null;

        return user.PasswordHash == HashPassword(password) ? user : null;
    }

    public bool ChangePassword(int userId, string currentPassword, string newPassword)
    {
        var user = _context.AppUsers.FirstOrDefault(u => u.Id == userId);
        if (user == null) return false;

        if (user.PasswordHash != HashPassword(currentPassword)) return false;

        user.PasswordHash = HashPassword(newPassword);
        _context.SaveChanges();
        return true;
    }

    // SHA256 hex string — good enough for a student project, not for production.
    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
