using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Interfaces;

public interface IPhotographerService
{
    List<Photographer> GetAll();
    Photographer? GetById(int id);
    Photographer Create(Photographer photographer);
    bool Update(int id, Photographer photographer);
    bool Delete(int id);
}
