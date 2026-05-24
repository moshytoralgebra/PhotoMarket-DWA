using PhotoMarket.DAL.Models;

namespace WebAPI.Models;

public class PhotoRequest
{
    public Photo Photo { get; set; } = null!;
    public List<int>? TagIds { get; set; }
}
