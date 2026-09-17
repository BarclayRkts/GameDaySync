using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repositories;

public interface ITrackedTeamsRepository
{
    Task<List<TrackedTeam>> GetAllAsync();
    Task<TrackedTeam?> GetBySportsDbIdAsync(int sportsDbId);
    Task<int[]> GetActiveSportsDbIdsAsync();
    Task<TrackedTeam> AddAsync(TrackedTeam trackedTeam);
    Task<bool> DeleteAsync(int id);
    Task<int> GetActiveCountAsync();
}

public class TrackedTeamsRepository(AppDbContext dbContext) : ITrackedTeamsRepository
{
    public Task<List<TrackedTeam>> GetAllAsync() =>
        dbContext.TrackedTeams.AsNoTracking().OrderByDescending(t => t.DateAdded).ToListAsync();

    public Task<TrackedTeam?> GetBySportsDbIdAsync(int sportsDbId) =>
        dbContext.TrackedTeams.FirstOrDefaultAsync(t => t.SportsDbId == sportsDbId);

    public Task<int[]> GetActiveSportsDbIdsAsync() =>
        dbContext.TrackedTeams
            .AsNoTracking()
            .Where(t => t.IsActive)
            .Select(t => t.SportsDbId)
            .ToArrayAsync();

    public async Task<TrackedTeam> AddAsync(TrackedTeam trackedTeam)
    {
        dbContext.TrackedTeams.Add(trackedTeam);
        await dbContext.SaveChangesAsync();
        return trackedTeam;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var trackedTeam = await dbContext.TrackedTeams.FindAsync(id);
        if (trackedTeam is null) return false;

        dbContext.TrackedTeams.Remove(trackedTeam);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public Task<int> GetActiveCountAsync() => dbContext.TrackedTeams.CountAsync(t => t.IsActive);
}
