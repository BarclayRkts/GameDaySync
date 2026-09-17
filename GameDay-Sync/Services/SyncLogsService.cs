using GameDay_Sync.Models;
using GameDay_Sync.Repositories;

namespace GameDay_Sync.Services;

public interface ISyncLogsService
{
    Task<List<SyncLogDto>> GetRecentAsync(int take = 30);
}

public class SyncLogsService(ISyncLogsRepository syncLogsRepository) : ISyncLogsService
{
    public async Task<List<SyncLogDto>> GetRecentAsync(int take = 30)
    {
        var logs = await syncLogsRepository.GetRecentAsync(take);
        return logs.Select(l => new SyncLogDto(l.Id, l.RunAt, l.Status, l.GamesAdded, l.DurationMs, l.Message)).ToList();
    }
}
