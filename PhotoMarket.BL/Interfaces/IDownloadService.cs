using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Interfaces;

public interface IDownloadService
{
    Download Create(int userId, int photoId);
    List<Download> GetByUser(int userId);
    List<Download> GetByPhoto(int photoId);
    List<Download> GetAllWithDetails();
}
