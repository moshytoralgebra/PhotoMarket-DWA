using PhotoMarket.DAL.Models;
using WebAPI.Models;

namespace WebAPI.Mapping;

public static class DownloadMappings
{
    public static DownloadDto ToDto(this Download download)
    {
        return new DownloadDto
        {
            Id = download.Id,
            UserId = download.UserId,
            UserName = download.User?.UserName,
            PhotoId = download.PhotoId,
            PhotoName = download.Photo?.Name,
            PhotoTitle = download.Photo?.Title,
            DownloadedAt = download.DownloadedAt
        };
    }
}
