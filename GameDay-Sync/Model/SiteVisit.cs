using System.ComponentModel.DataAnnotations;

namespace GameDay_Sync.Model;

public class SiteVisit
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string VisitorId { get; set; } = string.Empty;

    [Required]
    public string PagePath { get; set; } = string.Empty;

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? Referrer { get; set; }

    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
}
