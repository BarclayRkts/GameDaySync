using System.Text.Json;
using GameDay_Sync.Model;

namespace GameDay_Sync.Services.Extraction;

public class ExtractionService(HttpClient httpClient)
{
    public async Task<SportsDbTeam> FindTeamByNameAsync(string teamName)
    {
        var normalizedName = teamName.Trim();
        var response = await httpClient.GetFromJsonAsync<SportsDbTeamSearchResponse>(
            $"https://www.thesportsdb.com/api/v1/json/3/searchteams.php?t={Uri.EscapeDataString(normalizedName)}");
        var teams = response?.Teams ?? [];

        var team = teams.FirstOrDefault(t =>
            t.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase))
            ?? (teams.Count == 1 ? teams[0] : null);

        if (team is null || !int.TryParse(team.IdTeam, out _))
        {
            throw new InvalidOperationException(
                teams.Count == 0
                    ? $"No SportsDB team was found for '{normalizedName}'."
                    : $"Multiple SportsDB teams match '{normalizedName}'. Enter the team's exact name.");
        }

        return team;
    }

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
        
        return allTeamEvents;
    }
    
}