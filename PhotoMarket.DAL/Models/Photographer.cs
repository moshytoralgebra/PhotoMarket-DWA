using System;
using System.Collections.Generic;

namespace PhotoMarket.DAL.Models;

public partial class Photographer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Bio { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
