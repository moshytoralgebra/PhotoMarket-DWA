using PhotoMarket.DAL.Models;
using WebAPI.Models;

namespace WebAPI.Mapping;

public static class TagMappings
{
    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }
}
