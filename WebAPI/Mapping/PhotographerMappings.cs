using PhotoMarket.DAL.Models;
using WebAPI.Models;

namespace WebAPI.Mapping;

public static class PhotographerMappings
{
    public static PhotographerDto ToDto(this Photographer photographer)
    {
        return new PhotographerDto
        {
            Id = photographer.Id,
            Name = photographer.Name,
            Email = photographer.Email,
            Bio = photographer.Bio,
            Website = photographer.Website
        };
    }
}
