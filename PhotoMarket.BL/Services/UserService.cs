using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Services;

public class UserService : IUserService
{
    private readonly PhotoMarketContext _context;

    public UserService(PhotoMarketContext context)
    {
        _context = context;
    }

    public List<AppUser> GetAll()
    {
        return _context.AppUsers
            .Include(u => u.Role)
            .ToList();
    }

    public AppUser? GetById(int id)
    {
        return _context.AppUsers
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Id == id);
    }

    public AppUser? GetByUserName(string userName)
    {
        return _context.AppUsers
            .Include(u => u.Role)
            .FirstOrDefault(u => u.UserName == userName);
    }

    public List<AppUser> GetUsersWithDownloads()
    {
        return _context.AppUsers
            .Include(u => u.Role)
            .Include(u => u.Downloads).ThenInclude(d => d.Photo)
            .Where(u => u.Downloads.Count > 0)
            .ToList();
    }

    public bool UpdateProfile(int id, string firstName, string lastName, string email, string? phoneNumber)
    {
        var user = _context.AppUsers.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Name = $"{firstName} {lastName}";
        user.Email = email;
        user.PhoneNumber = phoneNumber;

        _context.SaveChanges();
        return true;
    }
}
