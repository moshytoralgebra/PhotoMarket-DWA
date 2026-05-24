using PhotoMarket.DAL.Models;
using WebAPI.Models;

namespace WebAPI.Mapping;

public static class PhotoMappings
{
    public static PhotoDto ToDto(this Photo photo)
    {
        return new PhotoDto
        {
            Id = photo.Id,
            Name = photo.Name,
            Title = photo.Title,
            Description = photo.Description,
            Price = photo.Price,
            ImagePath = photo.ImagePath,
            UploadDate = photo.UploadDate,
            PhotographerId = photo.PhotographerId,
            PhotographerName = photo.Photographer?.Name,
            Tags = photo.PhotoTags?
                .Where(pt => pt.Tag != null)
                .Select(pt => pt.Tag.Name)
                .ToList() ?? new List<string>()
        };
    }
}
