namespace GameDay_Sync.Models;

public record GameDto(
    string Id,
    string EventName,
    string League,
    DateTime GameTime,
    string TeamName,
    string OpponentName,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class GamesQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? League { get; set; }
}
