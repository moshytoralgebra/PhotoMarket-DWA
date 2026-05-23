using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Services;

public class PhotoService : IPhotoService
{
    private readonly PhotoMarketContext _context;
    private readonly ILogService _logService;

    public PhotoService(PhotoMarketContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }

    public List<Photo> GetAll()
    {
        return _context.Photos
            .Include(p => p.Photographer)
            .Include(p => p.PhotoTags).ThenInclude(pt => pt.Tag)
            .ToList();
    }

    public Photo? GetById(int id)
    {
        return _context.Photos
            .Include(p => p.Photographer)
            .Include(p => p.PhotoTags).ThenInclude(pt => pt.Tag)
            .FirstOrDefault(p => p.Id == id);
    }

    // searchTerm matches against Name, Title, and Description
    public List<Photo> Search(string? searchTerm, int? photographerId, int? tagId, int page, int pageSize)
    {
        return BuildSearchQuery(searchTerm, photographerId, tagId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int CountSearchResults(string? searchTerm, int? photographerId, int? tagId)
    {
        return BuildSearchQuery(searchTerm, photographerId, tagId).Count();
    }

    private IQueryable<Photo> BuildSearchQuery(string? searchTerm, int? photographerId, int? tagId)
    {
        var query = _context.Photos
            .Include(p => p.Photographer)
            .Include(p => p.PhotoTags).ThenInclude(pt => pt.Tag)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Title.ToLower().Contains(term) ||
                p.Description.ToLower().Contains(term));
        }

        if (photographerId.HasValue)
            query = query.Where(p => p.PhotographerId == photographerId.Value);

        if (tagId.HasValue)
            query = query.Where(p => p.PhotoTags.Any(pt => pt.TagId == tagId.Value));

        return query;
    }

    public Photo Create(Photo photo, List<int>? tagIds)
    {
        photo.UploadDate = DateTime.UtcNow;
        _context.Photos.Add(photo);
        _context.SaveChanges();

        if (tagIds != null && tagIds.Count > 0)
        {
            foreach (var tagId in tagIds)
                _context.PhotoTags.Add(new PhotoTag { PhotoId = photo.Id, TagId = tagId });

            _context.SaveChanges();
        }

        _logService.Add("Info", $"Photo created: {photo.Name} (Id={photo.Id})");
        return photo;
    }

    public bool Update(int id, Photo updated, List<int>? tagIds)
    {
        var photo = _context.Photos
            .Include(p => p.PhotoTags)
            .FirstOrDefault(p => p.Id == id);

        if (photo == null) return false;

        photo.Name = updated.Name;
        photo.Title = updated.Title;
        photo.Description = updated.Description;
        photo.Price = updated.Price;
        photo.ImagePath = updated.ImagePath;
        photo.PhotographerId = updated.PhotographerId;

        // Replace all tag assignments when a new list is provided
        if (tagIds != null)
        {
            _context.PhotoTags.RemoveRange(photo.PhotoTags);
            foreach (var tagId in tagIds)
                _context.PhotoTags.Add(new PhotoTag { PhotoId = id, TagId = tagId });
        }

        _context.SaveChanges();
        _logService.Add("Info", $"Photo updated: Id={id}");
        return true;
    }

    public bool Delete(int id)
    {
        var photo = _context.Photos
            .Include(p => p.PhotoTags)
            .Include(p => p.Downloads)
            .FirstOrDefault(p => p.Id == id);

        if (photo == null) return false;

        // Remove dependent rows before removing the photo
        _context.PhotoTags.RemoveRange(photo.PhotoTags);
        _context.Downloads.RemoveRange(photo.Downloads);
        _context.Photos.Remove(photo);
        _context.SaveChanges();

        _logService.Add("Info", $"Photo deleted: Id={id}");
        return true;
    }
}
