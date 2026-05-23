using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Services;

public class TagService : ITagService
{
    private readonly PhotoMarketContext _context;

    public TagService(PhotoMarketContext context)
    {
        _context = context;
    }

    public List<Tag> GetAll()
    {
        return _context.Tags.ToList();
    }

    public Tag? GetById(int id)
    {
        return _context.Tags.FirstOrDefault(t => t.Id == id);
    }

    public Tag Create(Tag tag)
    {
        _context.Tags.Add(tag);
        _context.SaveChanges();
        return tag;
    }

    public bool Update(int id, Tag updated)
    {
        var tag = _context.Tags.FirstOrDefault(t => t.Id == id);
        if (tag == null) return false;

        tag.Name = updated.Name;
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var tag = _context.Tags
            .Include(t => t.PhotoTags)
            .FirstOrDefault(t => t.Id == id);

        if (tag == null) return false;

        // Remove all photo-tag join records before deleting the tag
        _context.PhotoTags.RemoveRange(tag.PhotoTags);
        _context.Tags.Remove(tag);
        _context.SaveChanges();
        return true;
    }
}
