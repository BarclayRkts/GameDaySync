using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repositories;

public interface ISyncLogsRepository
{
    Task<List<SyncLog>> GetRecentAsync(int take = 30);
    Task<(List<SyncLog> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<SyncLog?> GetLatestAsync();
    Task<SyncLog> AddAsync(SyncLog log);
}

public class SyncLogsRepository(AppDbContext dbContext) : ISyncLogsRepository
{
    public Task<List<SyncLog>> GetRecentAsync(int take = 30) =>
        dbContext.SyncLogs.AsNoTracking().OrderByDescending(l => l.RunAt).Take(take).ToListAsync();

    public async Task<(List<SyncLog> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = dbContext.SyncLogs.AsNoTracking().OrderByDescending(l => l.RunAt);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalCount);
    }

    public Task<SyncLog?> GetLatestAsync() =>
        dbContext.SyncLogs.AsNoTracking().OrderByDescending(l => l.RunAt).FirstOrDefaultAsync();

    public async Task<SyncLog> AddAsync(SyncLog log)
    {
        dbContext.SyncLogs.Add(log);
        await dbContext.SaveChangesAsync();
        return log;
    }
}
