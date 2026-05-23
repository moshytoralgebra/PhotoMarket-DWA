using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Interfaces;

public interface ITagService
{
    List<Tag> GetAll();
    Tag? GetById(int id);
    Tag Create(Tag tag);
    bool Update(int id, Tag tag);
    bool Delete(int id);
}
