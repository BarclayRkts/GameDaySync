using GameDay_Sync.Models;
using GameDay_Sync.Repositories;

namespace GameDay_Sync.Services;

public interface ISyncLogsService
{
    Task<List<SyncLogDto>> GetRecentAsync(int take = 30);
    Task<PagedResult<SyncLogDto>> GetPagedAsync(SyncLogsQuery query);
}

public class SyncLogsService(ISyncLogsRepository syncLogsRepository) : ISyncLogsService
{
    public async Task<List<SyncLogDto>> GetRecentAsync(int take = 30)
    {
        var logs = await syncLogsRepository.GetRecentAsync(take);
        return logs.Select(l => new SyncLogDto(l.Id, l.RunAt, l.Status, l.GamesAdded, l.DurationMs, l.Message)).ToList();
    }

    public async Task<PagedResult<SyncLogDto>> GetPagedAsync(SyncLogsQuery query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var (items, totalCount) = await syncLogsRepository.GetPagedAsync(page, pageSize);

        return new PagedResult<SyncLogDto>
        {
            Items = items.Select(l => new SyncLogDto(l.Id, l.RunAt, l.Status, l.GamesAdded, l.DurationMs, l.Message)).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
        };
    }
}
