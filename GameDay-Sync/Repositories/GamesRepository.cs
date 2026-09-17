using GameDay_Sync.Data;
using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Repositories;

public interface IGamesRepository
{
    Task<(List<Game> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, string? league);
    Task<int> GetTotalCountAsync();
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
}
