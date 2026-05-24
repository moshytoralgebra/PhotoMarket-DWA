namespace WebAPI.Models;

public class PhotographerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }
    public string? Website { get; set; }
}
