using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repositories;

public interface IGamesRepository
{
    Task<(List<Game> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, string? league);
    Task<int> GetTotalCountAsync();
    Task<List<Game>> GetDailyGames(bool includeAlreadyAlerted = false);
    Task<List<Game>> GetWeeklyGames(bool includeAlreadyAlerted = false);
    Task SaveTrackingChanges();
}

public class GamesRepository(AppDbContext dbContext) : IGamesRepository
{
    public async Task<(List<Game> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, string? league)
    {
        IQueryable<Game> query = dbContext.Games.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(league) && !league.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(g => g.League == league);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(g =>
                EF.Functions.ILike(g.EventName, $"%{term}%") ||
                EF.Functions.ILike(g.TeamName, $"%{term}%") ||
                EF.Functions.ILike(g.OpponentName, $"%{term}%") ||
                EF.Functions.ILike(g.Id, $"%{term}%"));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(g => g.GameTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public Task<int> GetTotalCountAsync() => dbContext.Games.CountAsync();

    public async Task<List<Game>> GetDailyGames(bool includeAlreadyAlerted = false)
    {
        var startUtc = GetCentralDayStartUtc();
        var endUtc = GetCentralDayStartUtc(daysFromToday: 1);

        // start of today through, the end of today
        return await dbContext.Games
            .Where(t => t.GameTime >= startUtc &&
                        t.GameTime < endUtc &&
                        (includeAlreadyAlerted || !t.DayAlertSent))
            .OrderBy(g => g.GameTime)
            .ToListAsync();
    }

    public async Task<List<Game>> GetWeeklyGames(bool includeAlreadyAlerted = false)
    {
        var startUtc = GetCentralDayStartUtc();
        var weekEndUtc = GetCentralDayStartUtc(daysFromToday: 7);

        // start of today through, but not including, the start of the eighth calendar day
        return await dbContext.Games
            .Where(t => t.GameTime >= startUtc &&
                        t.GameTime < weekEndUtc &&
                        (includeAlreadyAlerted || !t.WeekAlertSent))
            .OrderBy(g => g.GameTime)
            .ToListAsync();
    }

    public async Task SaveTrackingChanges()
    {
        await dbContext.SaveChangesAsync();
    }

    private static DateTime GetCentralDayStartUtc(int daysFromToday = 0)
    {
        var centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        var todayCentral = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, centralTimeZone).Date;
        return TimeZoneInfo.ConvertTimeToUtc(todayCentral.AddDays(daysFromToday), centralTimeZone);
    }
}
