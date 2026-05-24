namespace WebAPI.Models;

public class PhotoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string ImagePath { get; set; } = null!;
    public DateTime UploadDate { get; set; }
    public int PhotographerId { get; set; }
    public string? PhotographerName { get; set; }
    public List<string> Tags { get; set; } = new();
}
