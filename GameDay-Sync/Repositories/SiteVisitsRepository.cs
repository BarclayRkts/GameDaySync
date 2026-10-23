using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repositories;

public interface ISiteVisitsRepository
{
    Task<SiteVisit> AddAsync(SiteVisit siteVisit);
    Task<int> GetTotalCountAsync();
    Task<int> GetUniqueVisitorCountAsync();
    Task<int> GetTodayCountAsync();
    Task<List<SiteVisit>> GetRecentAsync(int count);
}

public class SiteVisitsRepository(AppDbContext dbContext) : ISiteVisitsRepository
{
    public async Task<SiteVisit> AddAsync(SiteVisit siteVisit)
    {
        dbContext.SiteVisits.Add(siteVisit);
        await dbContext.SaveChangesAsync();
        return siteVisit;
    }

    public Task<int> GetTotalCountAsync() => dbContext.SiteVisits.CountAsync();

    public Task<int> GetUniqueVisitorCountAsync() =>
        dbContext.SiteVisits.Select(v => v.VisitorId).Distinct().CountAsync();

    public Task<int> GetTodayCountAsync()
    {
        var todayUtc = DateTime.UtcNow.Date;
        return dbContext.SiteVisits.CountAsync(v => v.VisitedAt >= todayUtc);
    }

    public Task<List<SiteVisit>> GetRecentAsync(int count) =>
        dbContext.SiteVisits
            .AsNoTracking()
            .OrderByDescending(v => v.VisitedAt)
            .Take(count)
            .ToListAsync();
}
