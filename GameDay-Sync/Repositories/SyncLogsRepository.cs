using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repositories;

public interface ISyncLogsRepository
{
    Task<List<SyncLog>> GetRecentAsync(int take = 30);
    Task<SyncLog?> GetLatestAsync();
    Task<SyncLog> AddAsync(SyncLog log);
}

public class SyncLogsRepository(AppDbContext dbContext) : ISyncLogsRepository
{
    public Task<List<SyncLog>> GetRecentAsync(int take = 30) =>
        dbContext.SyncLogs.AsNoTracking().OrderByDescending(l => l.RunAt).Take(take).ToListAsync();

    public Task<SyncLog?> GetLatestAsync() =>
        dbContext.SyncLogs.AsNoTracking().OrderByDescending(l => l.RunAt).FirstOrDefaultAsync();

    public async Task<SyncLog> AddAsync(SyncLog log)
    {
        dbContext.SyncLogs.Add(log);
        await dbContext.SaveChangesAsync();
        return log;
    }
}
