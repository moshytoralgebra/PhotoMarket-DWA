using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Services;

public class PhotographerService : IPhotographerService
{
    private readonly PhotoMarketContext _context;

    public PhotographerService(PhotoMarketContext context)
    {
        _context = context;
    }

    public List<Photographer> GetAll()
    {
        return _context.Photographers.ToList();
    }

    public Photographer? GetById(int id)
    {
        return _context.Photographers
            .Include(ph => ph.Photos)
            .FirstOrDefault(ph => ph.Id == id);
    }

    public Photographer Create(Photographer photographer)
    {
        _context.Photographers.Add(photographer);
        _context.SaveChanges();
        return photographer;
    }

    public bool Update(int id, Photographer updated)
    {
        var photographer = _context.Photographers.FirstOrDefault(ph => ph.Id == id);
        if (photographer == null) return false;

        photographer.Name = updated.Name;
        photographer.Email = updated.Email;
        photographer.Bio = updated.Bio;
        photographer.Website = updated.Website;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var photographer = _context.Photographers
            .Include(ph => ph.Photos)
            .FirstOrDefault(ph => ph.Id == id);

        if (photographer == null) return false;

        // Block deletion if photos still reference this photographer
        if (photographer.Photos.Count > 0)
            return false;

        _context.Photographers.Remove(photographer);
        _context.SaveChanges();
        return true;
    }
}
