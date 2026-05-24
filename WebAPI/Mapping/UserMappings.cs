using PhotoMarket.DAL.Models;
using WebAPI.Models;

namespace WebAPI.Mapping;

public static class UserMappings
{
    public static UserDto ToDto(this AppUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            RoleName = user.Role?.Name
        };
    }
}
