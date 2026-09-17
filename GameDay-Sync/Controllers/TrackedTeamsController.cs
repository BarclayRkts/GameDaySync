using GameDay_Sync.Models;
using GameDay_Sync.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameDay_Sync.Controllers;

[ApiController]
[Route("api/tracked-teams")]
public class TrackedTeamsController(ITrackedTeamsService trackedTeamsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TrackedTeamDto>>> GetTrackedTeams()
    {
        return Ok(await trackedTeamsService.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<TrackedTeamDto>> AddTrackedTeam([FromBody] AddTrackedTeamRequest request)
    {
        try
        {
            var created = await trackedTeamsService.AddAsync(request);
            return CreatedAtAction(nameof(GetTrackedTeams), new { }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTrackedTeam(int id)
    {
        var deleted = await trackedTeamsService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
