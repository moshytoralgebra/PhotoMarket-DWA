using System;
using System.Collections.Generic;

namespace PhotoMarket.DAL.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PhotoTag> PhotoTags { get; set; } = new List<PhotoTag>();
}
