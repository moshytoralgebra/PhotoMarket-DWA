using Microsoft.EntityFrameworkCore;
using PhotoMarket.BL.Interfaces;
using PhotoMarket.DAL.Models;

namespace PhotoMarket.BL.Services;

public class LogService : ILogService
{
    private readonly PhotoMarketContext _context;

    public LogService(PhotoMarketContext context)
    {
        _context = context;
    }

    public void Add(string level, string message)
    {
        _context.Logs.Add(new Log
        {
            Level = level,
            Message = message,
            Timestamp = DateTime.UtcNow
        });
        _context.SaveChanges();
    }

    public List<Log> GetLast(int count)
    {
        return _context.Logs
            .OrderByDescending(l => l.Timestamp)
            .Take(count)
            .ToList();
    }

    public int Count()
    {
        return _context.Logs.Count();
    }
}
