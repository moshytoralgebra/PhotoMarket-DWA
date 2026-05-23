using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Interfaces;

public interface ILogService
{
    void Add(string level, string message);
}
