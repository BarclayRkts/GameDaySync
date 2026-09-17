using GameDay_Sync.Models;
using GameDay_Sync.Repositories;

namespace GameDay_Sync.Services;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
}

public class DashboardService(
    IGamesRepository gamesRepository,
    ITrackedTeamsRepository trackedTeamsRepository,
    ISyncLogsRepository syncLogsRepository) : IDashboardService
{
    private static readonly TimeZoneInfo CentralTimeZone = ResolveCentralTimeZone();

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var totalGames = await gamesRepository.GetTotalCountAsync();
        var activeTrackedTeams = await trackedTeamsRepository.GetActiveCountAsync();
        var recentLogs = await syncLogsRepository.GetRecentAsync(7);
        var latest = recentLogs.FirstOrDefault();

        var status = latest is null
            ? "Active"
            : latest.Status switch
            {
                "Failed" => "Down",
                "Partial" => "Degraded",
                _ => "Active",
            };

        return new DashboardSummaryDto
        {
            TotalSyncedGames = totalGames,
            SystemStatus = status,
            LastRunAt = latest?.RunAt,
            NextRunAt = GetNextScheduledRunUtc(),
            ActiveTrackedTeams = activeTrackedTeams,
            LatencySeries = recentLogs
                .OrderBy(l => l.RunAt)
                .Select(l => new LatencyPointDto(l.RunAt, l.DurationMs, l.GamesAdded))
                .ToList(),
        };
    }

    private static DateTime GetNextScheduledRunUtc()
    {
        var nowCentral = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, CentralTimeZone);
        var todayEightAm = nowCentral.Date.AddHours(8);
        var nextRunCentral = nowCentral <= todayEightAm ? todayEightAm : todayEightAm.AddDays(1);

        return TimeZoneInfo.ConvertTimeToUtc(nextRunCentral, CentralTimeZone);
    }

    private static TimeZoneInfo ResolveCentralTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        }
    }
}
