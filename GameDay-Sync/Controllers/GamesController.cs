using GameDay_Sync.Models;
using GameDay_Sync.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameDay_Sync.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController(IGamesQueryService gamesQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<GameDto>>> GetGames([FromQuery] GamesQuery query)
    {
        var result = await gamesQueryService.GetGamesAsync(query);
        return Ok(result);
    }
}
