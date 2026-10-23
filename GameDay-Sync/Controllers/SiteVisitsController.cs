using GameDay_Sync.Models;
using GameDay_Sync.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameDay_Sync.Controllers;

[ApiController]
[Route("api/site-visits")]
public class SiteVisitsController(ISiteVisitsService siteVisitsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RecordVisit([FromBody] RecordVisitRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers.UserAgent.ToString();

        await siteVisitsService.RecordVisitAsync(request, ipAddress, userAgent);
        return NoContent();
    }

    [HttpGet("stats")]
    public async Task<ActionResult<VisitStatsDto>> GetStats()
    {
        return Ok(await siteVisitsService.GetStatsAsync());
    }
}
