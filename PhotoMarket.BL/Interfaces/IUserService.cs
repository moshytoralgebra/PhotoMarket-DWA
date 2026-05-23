using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Interfaces;

public interface IUserService
{
    List<AppUser> GetAll();
    AppUser? GetById(int id);
    AppUser? GetByUserName(string userName);
    List<AppUser> GetUsersWithDownloads();
    bool UpdateProfile(int id, string firstName, string lastName, string email, string? phoneNumber);
}
