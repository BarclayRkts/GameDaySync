namespace GameDay_Sync.Models;

public class DashboardSummaryDto
{
    public int TotalSyncedGames { get; init; }
    public string SystemStatus { get; init; } = "Active";
    public DateTime? LastRunAt { get; init; }
    public DateTime NextRunAt { get; init; }
    public int ActiveTrackedTeams { get; init; }
    public IReadOnlyList<LatencyPointDto> LatencySeries { get; init; } = [];
}

public record LatencyPointDto(DateTime Day, long LatencyMs, int GamesAdded);
