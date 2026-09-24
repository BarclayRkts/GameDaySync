using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repository;

public class GamesRepo(AppDbContext appDbContext)
{
    public async Task<List<Game>> GetDailyGames()
    {
        var startUtc = GetCentralDayStartUtc();
        var endUtc = GetCentralDayStartUtc(daysFromToday: 1);
        
        // start of today through, the end of today
        return await appDbContext.Games
            .Where(t => t.GameTime >= startUtc &&
                        t.GameTime < endUtc &&
                        !t.DayAlertSent)
            .OrderBy(g => g.GameTime)
            .ToListAsync();
    }

    public async Task<List<Game>> GetWeeklyGames()
    {
        var startUtc = GetCentralDayStartUtc();
        var weekEndUtc = GetCentralDayStartUtc(daysFromToday: 7);

        // start of today through, but not including, the start of the eighth calendar day
        return await appDbContext.Games
            .Where(t => t.GameTime >= startUtc &&
                        t.GameTime < weekEndUtc &&
                        !t.WeekAlertSent)
            .OrderBy(g => g.GameTime)
            .ToListAsync();
    }

    public async Task SaveTrackingChanges()
    {
        await appDbContext.SaveChangesAsync();
    }

    private static DateTime GetCentralDayStartUtc(int daysFromToday = 0)
    {
        var centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        var todayCentral = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, centralTimeZone).Date;
        return TimeZoneInfo.ConvertTimeToUtc(todayCentral.AddDays(daysFromToday), centralTimeZone);
    }
}