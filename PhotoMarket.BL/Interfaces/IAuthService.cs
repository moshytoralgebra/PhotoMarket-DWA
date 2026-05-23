using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Interfaces;

public interface IAuthService
{
    AppUser? Register(string userName, string email, string firstName, string lastName, string password);
    AppUser? ValidateLogin(string userName, string password);
    bool ChangePassword(int userId, string currentPassword, string newPassword);
}
