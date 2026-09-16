using GameDay_Sync.Model;
using GameDay_Sync.Service.Extraction;

namespace GameDay_Sync.Service;

public class Pipeline(ExtractionService extractionService)
{
    public async Task RunPipeline()
    {
        int[] ids = [135256, 133616, 134876, 134926];
        List <SportsDbEvent> games = await extractionService.GetTeamGames(ids);
        
    }
}