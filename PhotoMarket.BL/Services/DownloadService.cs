using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Services;

public class DownloadService : IDownloadService
{
    private readonly PhotoMarketContext _context;

    public DownloadService(PhotoMarketContext context)
    {
        _context = context;
    }

    public Download Create(int userId, int photoId)
    {
        var download = new Download
        {
            UserId = userId,
            PhotoId = photoId,
            DownloadedAt = DateTime.UtcNow
        };
        _context.Downloads.Add(download);
        _context.SaveChanges();
        return download;
    }

    public List<Download> GetByUser(int userId)
    {
        return _context.Downloads
            .Include(d => d.Photo).ThenInclude(p => p.Photographer)
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.DownloadedAt)
            .ToList();
    }

    public List<Download> GetByPhoto(int photoId)
    {
        return _context.Downloads
            .Include(d => d.User)
            .Where(d => d.PhotoId == photoId)
            .OrderByDescending(d => d.DownloadedAt)
            .ToList();
    }

    public List<Download> GetAllWithDetails()
    {
        return _context.Downloads
            .Include(d => d.User)
            .Include(d => d.Photo).ThenInclude(p => p.Photographer)
            .OrderByDescending(d => d.DownloadedAt)
            .ToList();
    }
}
