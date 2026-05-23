using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Interfaces;

public interface IPhotoService
{
    List<Photo> GetAll();
    Photo? GetById(int id);
    List<Photo> Search(string? searchTerm, int? photographerId, int? tagId, int page, int pageSize);
    int CountSearchResults(string? searchTerm, int? photographerId, int? tagId);
    Photo Create(Photo photo, List<int>? tagIds);
    bool Update(int id, Photo photo, List<int>? tagIds);
    bool Delete(int id);
}
