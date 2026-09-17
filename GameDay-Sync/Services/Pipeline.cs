using GameDay_Sync.Model;
using GameDay_Sync.Repositories;
using GameDay_Sync.Services.Extraction;
using GameDay_Sync.Services.Load;
using GameDay_Sync.Services.Notifications;
using System.Diagnostics;

namespace GameDay_Sync.Services;

public class Pipeline(
    ExtractionService extractionService,
    LoadService loadService,
    NotificationsEngine notificationsEngine,
    ITrackedTeamsRepository trackedTeamsRepository,
    ISyncLogsRepository syncLogsRepository)
{
    public async Task RunPipeline(string alertMode)
    {
        var stopwatch = Stopwatch.StartNew();
        var gamesAdded = 0;

        try
        {
            var trackedTeamIds = await trackedTeamsRepository.GetActiveSportsDbIdsAsync();
            List<SportsDbEvent> games = await extractionService.GetTeamGames(trackedTeamIds);
            gamesAdded = await loadService.SaveGames(games);

            if (alertMode.Equals("--weekly", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Executing Weekly Notification Engine...");
                await notificationsEngine.SendWeeklyNotifications();
            }
            else if (alertMode.Equals("--daily", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Executing Daily Notification Engine...");
                await notificationsEngine.SendDailyNotifications();
            }
            else if (!alertMode.Equals("--sync-only", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Invalid mode selection: '{alertMode}'. Use '--weekly', '--daily', or '--sync-only'.", nameof(alertMode));
            }

            await RecordRunAsync("Success", gamesAdded, stopwatch.ElapsedMilliseconds, $"Processed {games.Count} events.");
        }
        catch (HttpRequestException ex)
        {
            await RecordRunAsync("Failed", gamesAdded, stopwatch.ElapsedMilliseconds, ex.Message);
            throw;
        }
        catch (System.Text.Json.JsonException ex)
        {
            await RecordRunAsync("Failed", gamesAdded, stopwatch.ElapsedMilliseconds, ex.Message);
            throw;
        }
        catch (FormatException ex)
        {
            await RecordRunAsync("Failed", gamesAdded, stopwatch.ElapsedMilliseconds, ex.Message);
            throw;
        }
    }

    private Task RecordRunAsync(string status, int gamesAdded, long durationMs, string message) =>
        syncLogsRepository.AddAsync(new SyncLog
        {
            RunAt = DateTime.UtcNow,
            Status = status,
            GamesAdded = gamesAdded,
            DurationMs = durationMs,
            Message = message,
        });
}