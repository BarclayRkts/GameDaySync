using System.Text.Json;
using GameDay_Sync.Model;

namespace GameDay_Sync.Service.Extraction;

public class ExtractionService(HttpClient httpClient)
{
    public async Task<List<SportsDbEvent>> GetTeamGames(int[] ids)
    {
        List<SportsDbEvent> allTeamEvents = [];

        foreach (var id in ids)
        {
            string url = $"https://www.thesportsdb.com/api/v1/json/3/eventsnext.php?id={id}";
            var response = await httpClient.GetFromJsonAsync<SportsDbResponse>(url);


            if (response?.Events != null)
            {
                allTeamEvents.AddRange(response.Events);
            }
        }
        
        string jsonLog = JsonSerializer.Serialize(allTeamEvents, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(jsonLog);
        
        return allTeamEvents;
    }
    
}