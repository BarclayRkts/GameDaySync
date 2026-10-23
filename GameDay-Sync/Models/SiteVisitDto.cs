using System.ComponentModel.DataAnnotations;

namespace GameDay_Sync.Models;

public record SiteVisitDto(
    int Id,
    string VisitorId,
    string PagePath,
    string? IpAddress,
    string? Referrer,
    DateTime VisitedAt);

public class RecordVisitRequest
{
    [Required]
    [MaxLength(100)]
    public string VisitorId { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string PagePath { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Referrer { get; set; }
}

public record VisitStatsDto(
    int TotalVisits,
    int UniqueVisitors,
    int VisitsToday,
    IReadOnlyList<SiteVisitDto> RecentVisits);
