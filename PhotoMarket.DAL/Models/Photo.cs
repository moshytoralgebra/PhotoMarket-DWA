using System;
using System.Collections.Generic;

namespace PhotoMarket.DAL.Models;

public partial class Photo
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string ImagePath { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public int PhotographerId { get; set; }

    public virtual ICollection<Download> Downloads { get; set; } = new List<Download>();

    public virtual ICollection<PhotoTag> PhotoTags { get; set; } = new List<PhotoTag>();

    public virtual Photographer Photographer { get; set; } = null!;
}
