namespace WebAPI.Models;

public class DownloadDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public int PhotoId { get; set; }
    public string? PhotoName { get; set; }
    public string? PhotoTitle { get; set; }
    public DateTime DownloadedAt { get; set; }
}
