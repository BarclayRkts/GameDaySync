using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repository;

public class GamesRepo(AppDbContext appDbContext)
{
    public async Task<List<Game>> GetWeeklyGames(string? alertMode)
    {
        if (alertMode == "daily")
        {
            return await appDbContext.Games
                .Where(t => t.GameTime == DateTime.UtcNow && !t.DayAlertSent)
                .ToListAsync();
        }
        
        return await appDbContext.Games
            .Where(t => t.GameTime >= DateTime.UtcNow && t.GameTime <= DateTime.UtcNow.AddDays(7) && !t.WeekAlertSent)
            .OrderBy(g => g.GameTime)
            .ToListAsync();
    }
    
    public async Task SaveTrackingChanges()
    {
        await appDbContext.SaveChangesAsync();
    }
}