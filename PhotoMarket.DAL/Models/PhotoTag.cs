using System;
using System.Collections.Generic;

namespace PhotoMarket.DAL.Models;

public partial class PhotoTag
{
    public int Id { get; set; }

    public int PhotoId { get; set; }

    public int TagId { get; set; }

    public virtual Photo Photo { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
