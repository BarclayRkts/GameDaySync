using GameDay_Sync.Model;
using GameDay_Sync.Models;
using GameDay_Sync.Repositories;
using GameDay_Sync.Services.Extraction;
using GameDay_Sync.Services.Load;

namespace GameDay_Sync.Services;

public interface ITrackedTeamsService
{
    Task<List<TrackedTeamDto>> GetAllAsync();
    Task<TrackedTeamDto> AddAsync(AddTrackedTeamRequest request);
    Task<bool> DeleteAsync(int id);
}

public class TrackedTeamsService(
    ITrackedTeamsRepository trackedTeamsRepository,
    ExtractionService extractionService,
    LoadService loadService) : ITrackedTeamsService
{
    public async Task<List<TrackedTeamDto>> GetAllAsync()
    {
        var trackedTeams = await trackedTeamsRepository.GetAllAsync();
        return trackedTeams.Select(ToDto).ToList();
    }

    public async Task<TrackedTeamDto> AddAsync(AddTrackedTeamRequest request)
    {
        var team = await extractionService.FindTeamByNameAsync(request.Name);
        var sportsDbId = int.Parse(team.IdTeam);
        var existing = await trackedTeamsRepository.GetBySportsDbIdAsync(sportsDbId);
        if (existing is not null)
        {
            throw new InvalidOperationException($"{team.Name} is already tracked.");
        }
        
        var upcomingGames = await extractionService.GetTeamGames([sportsDbId]);
        var trackedTeam = new TrackedTeam
        {
            SportsDbId = sportsDbId,
            Name = team.Name,
            Category = "Team",
            IsActive = true,
            DateAdded = DateTime.UtcNow,
        };

        var created = await trackedTeamsRepository.AddAsync(trackedTeam);
        await loadService.SaveGames(upcomingGames);
        return ToDto(created);
    }

    public Task<bool> DeleteAsync(int id) => trackedTeamsRepository.DeleteAsync(id);

    private static TrackedTeamDto ToDto(TrackedTeam t) => new(
        t.Id,
        t.SportsDbId,
        t.Name,
        t.Category,
        t.DateAdded,
        t.IsActive ? "Active" : "Paused");
}
