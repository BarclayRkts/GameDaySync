using GameDay_Sync.Model;
using GameDay_Sync.Services.Extraction;
using GameDay_Sync.Services.Load;

namespace GameDay_Sync.Services;

public class Pipeline(ExtractionService extractionService, LoadService loadService)
{
    public async Task RunPipeline()
    {
        int[] ids = [135256, 133616, 134876, 134926];
        List<SportsDbEvent> games = await extractionService.GetTeamGames(ids);
        await loadService.SaveGames(games);
    }
}