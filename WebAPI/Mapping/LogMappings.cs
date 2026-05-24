using PhotoMarket.DAL.Models;
using WebAPI.Models;

namespace WebAPI.Mapping;

public static class LogMappings
{
    public static LogDto ToDto(this Log log)
    {
        return new LogDto
        {
            Id = log.Id,
            Timestamp = log.Timestamp,
            Level = log.Level,
            Message = log.Message
        };
    }
}
