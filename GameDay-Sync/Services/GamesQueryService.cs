using GameDay_Sync.Models;
using GameDay_Sync.Repositories;

namespace GameDay_Sync.Services;

public interface IGamesQueryService
{
    Task<PagedResult<GameDto>> GetGamesAsync(GamesQuery query);
}

public class GamesQueryService(IGamesRepository gamesRepository) : IGamesQueryService
{
    public async Task<PagedResult<GameDto>> GetGamesAsync(GamesQuery query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var (items, totalCount) = await gamesRepository.GetPagedAsync(page, pageSize, query.Search, query.League);

        var dtos = items.Select(g => new GameDto(
            g.Id,
            g.EventName,
            g.League,
            g.GameTime,
            g.TeamName,
            g.OpponentName,
            g.CreatedAt,
            g.UpdatedAt)).ToList();

        return new PagedResult<GameDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
        };
    }
}
