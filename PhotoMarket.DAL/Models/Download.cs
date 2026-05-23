using System;
using System.Collections.Generic;

namespace PhotoMarket.DAL.Models;

public partial class Download
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PhotoId { get; set; }

    public DateTime DownloadedAt { get; set; }

    public virtual Photo Photo { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
