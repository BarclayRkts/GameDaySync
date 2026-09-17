using GameDay_Sync.Models;
using GameDay_Sync.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameDay_Sync.Controllers;

[ApiController]
[Route("api/sync-logs")]
public class SyncLogsController(ISyncLogsService syncLogsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SyncLogDto>>> GetSyncLogs([FromQuery] int take = 30)
    {
        return Ok(await syncLogsService.GetRecentAsync(take));
    }
}
